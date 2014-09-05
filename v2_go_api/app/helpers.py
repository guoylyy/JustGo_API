# -*- coding: utf-8 -*-
import hashlib
import time as stime
from datetime import datetime, timedelta, time , date
from flask import current_app
from flask.ext.restful import abort
from app.models import User
from sqlalchemy.orm.exc import NoResultFound


def abort_if_category_doesnt_exist(category_name):
	if Category.query.filter(Category.category_name==category_name).count() == 0:
		abort(404, message="Category {} doesn't exist".format(category_name))

def abort_if_user_doesnt_exit(user_id):
	if User.query.filter(User.user_id==user_id).count() == 0:
		abort(404, message="User with ID {} doesn't exist".format(user_id))
	else:
		return 	User.query.filter(User.user_id==user_id).first()

def check_authorization():
	parser = reqparse.RequestParser()
	parser.add_argument('Authorization', type=str, location='headers')
	token = parser.parse_args()['Authorization']
	if not token:
		abort(500, message="Authorization Failed")
	else:
		# Check expired
		check_expire(token)
		u = User.query.filter(User.token==token).first()
		if u:
			return u
		else:
			abort(500, message='Invalid Authorization')

def _str2date(dstr):
	ym = [int(d) for d in dstr.split('-')]
	return date(ym[0],ym[1],ym[2])

def _str2time(tstr):
	tm = [int(t) for t in tstr.split(':')]
	return time(tm[0],tm[1])

def _mk_timestamp(datetime):
	return stime.mktime(datetime.timetuple())


def _find_or_create_thumbnail(obj, imageset, width=None, height=None):
	assert width is not None or height is not None 
	try:
		image = imageset.find_thumbnail(width=width, height=height)
	except NoResultFound:
		imageset.generate_thumbnail(width=width, height=height) 
		db.session.add(obj)
		db.session.commit()
		image = imageset.find_thumbnail(width=width, height=height)
	return image


def make_token(id):
    expire_day = current_app.config['SESSION_EXPIRE_DAYS']
    expire_time = (datetime.now() + timedelta(expire_day,0)).timetuple();
    expire_datetime = stime.mktime(expire_time)
    expire = str(int(expire_datetime))
    s = '%s:%s:%s' % (id, expire, current_app.config['MD5_RANDOM'])
    md5 = hashlib.md5(s.encode('utf-8')).hexdigest()
    return md5+":"+expire+":"+str(id)

def check_expire(token):
    '''
    '''
    try :
        md5, expire, id = token.split(":", 3)
        if int(expire) > stime.time():
            return False
        abort(500, message='Expire Token')
    except Exception as e:
        abort(500, message='Invalid Authorization')
