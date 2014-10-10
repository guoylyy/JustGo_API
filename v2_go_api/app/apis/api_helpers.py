# -*- coding: utf-8 -*-
import hashlib
import urllib
import time as stime
from datetime import datetime, timedelta, time , date
from flask import current_app
from flask.ext.restful import abort
from app.models import User, Category
from sqlalchemy.orm.exc import NoResultFound
from flask.ext.restful import  reqparse
from app.helpers import check_expire, make_token
from app.extensions import db


def abort_if_category_doesnt_exist(category_name):
	if Category.query.filter(Category.category_name==category_name).count() == 0:
		abort(404, message="Category {} doesn't exist".format(category_name))

def abort_if_user_doesnt_exit(user_id):
	if User.query.filter(User.user_id==user_id).count() == 0:
		abort(404, message="User with ID {} doesn't exist".format(user_id))
	else:
		return 	User.query.filter(User.user_id==user_id).first()

def internal_error():
	abort(500, message="Internal Error Happen")

def parameter_error():
	abort(500, message="Parameter Error")	

def get_user():
	parser = reqparse.RequestParser()
	parser.add_argument('Authorization', type=str, location='headers')
	token = parser.parse_args()['Authorization']
	if not token:
		return None
	else:
		token = urllib.unquote(parser.parse_args()['Authorization']).decode('utf8') 
		u = User.query.filter(User.token==token).first()
		if u:
			return u
		else:
			return None

def check_authorization():
	parser = reqparse.RequestParser()
	parser.add_argument('Authorization', type=str, location='headers')
	token = parser.parse_args()['Authorization']
	if not token:
		abort(500, message="Authorization Failed")
	else:
		token = urllib.unquote(parser.parse_args()['Authorization']).decode('utf8') 
		#check_result = check_expire(token)  #TO-DO should be open if app support
		#if check_result is not False:
		#	abort(500, message="Token Expire")
		u = User.query.filter(User.token==token).first()
		if u:
			return u 
		else:
			abort(500, message='Invalid Authorization')

def remake_token(id):
	u = User.query.filter(User.user_id==id).first()
	u.token = make_token(u.user_id)
	db.session.add(u)
	db.session.commit()
	return u