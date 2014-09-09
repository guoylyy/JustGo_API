from datetime import datetime
from sqlalchemy_imageattach.entity import Image, image_attachment, store_context
from flask.ext.restful import Resource, reqparse
from app.helpers import * 
from app.extensions import db, fs_store
from api_helpers import *

class UserRest(Resource):
	def get(self, user_id):
		""" Get the basic information of one user
		"""
		user = abort_if_user_doesnt_exit(user_id)
		return user.to_json(), 200

	def post(self):
		""" Update information of one user
		"""
		user = check_authorization()
		up = self.__user_update_parser()
		user.user_name = up['name']
		user.description = up['description']
		db.session.add(user)
		db.session.commit()
		return user.to_json(), 200

	def __user_update_parser(self):
		up = reqparse.RequestParser()
		up.add_argument('name', type=str, location='form')
		up.add_argument('description', type=str, location='form')
		return up.parse_args()

class ProfileRest(Resource):
	def get(self):
		user = check_authorization()
		return user.to_json(), 200

class FollowRest(Resource):
	"""
	"""
	def post(self, user_id):
		user = check_authorization()
		target_user = User.query.filter(User.user_id==user_id).first()
		target_user.fans.append(user)
		db.session.add(user)
		db.session.commit()
		return {'result':'success'}, 200

class UnFollowRest(Resource):
	"""
	"""
	def post(self, user_id):
		user = check_authorization()
		target_user = User.query.filter(User.user_id==user_id).first()
		target_user.fans.remove(user)
		db.session.add(target_user)
		db.session.commit()
		return {'result':'success'}, 200

class LoginRest(Resource):
	def post(self):
		""" Login restful api
		"""
		up = self.__user_parser()
		user = User.query.filter(User.facebook_token==up['facebook_token']).first()
		if user is None:
			#register
			token = "fdsafsafas"
			u = User(up['name'],up['description'],token,up['facebook_token'],'header')
			with store_context(fs_store):	
				with open('pic1.jpg','rb') as f:
					u.header_icon.from_blob(f.read())
				db.session.add(u)
				flag = db.session.commit()
			u.token = make_token(u.user_id)
			db.session.add(u)
			db.session.commit()
			return {'token': u.token}, 200
		else:
			check_expire(user.token)
			return {'token': user.token}, 200

	def __user_parser(self):
		up = reqparse.RequestParser()
		up.add_argument('facebook_token', type=str, location='form')
		up.add_argument('name', type=str, location='form')
		up.add_argument('description', type=str, location='form')
		return up.parse_args()