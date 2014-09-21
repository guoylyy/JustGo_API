from datetime import datetime
from sqlalchemy_imageattach.entity import Image, image_attachment, store_context
from flask.ext.restful import Resource, reqparse
from app.helpers import * 
from app.extensions import db, fs_store
from api_helpers import *
from requests import get

class UserRest(Resource):
	def get(self, user_id):
		""" 
			Get the basic information of one user
		"""
		u = check_authorization()
		user = abort_if_user_doesnt_exit(user_id)
		return user.to_json(u), 200

	def post(self):
		""" 
			Update information of one user
		"""
		user = check_authorization()
		up = self.__user_update_parser()
		user.name = up['name']
		user.description = up['description']
		if user.validate():
			db.session.add(user)
			db.session.commit()
			return {'result':'success'}, 200
		else:
			return {'result':'fail'}, 404

	def __user_update_parser(self):
		up = reqparse.RequestParser()
		up.add_argument('name', type=str, location='headers')
		up.add_argument('description', type=str, location='headers')
		return up.parse_args()

class ProfileRest(Resource):
	def get(self):
		user = check_authorization()
		return user.to_json(), 200

class FansRest(Resource):
	def get(self):
		u = check_authorization()
		return [f.header_json() for f in u.fans], 200

class FollowingsRest(Resource):
	def get(self):
		u = check_authorization()
		return [f.header_json() for f in u.followings], 200

class UserFansRest(Resource):
	def get(self, user_id):
		u = User.query.filter(User.user_id==user_id).first()
		if u:
			return [f.header_json() for f in u.fans], 200
		else:
			return [],200

class UserFollowingsRest(Resource):
	def get(self, user_id):
		u = User.query.filter(User.user_id==user_id).first()
		if u:
			return [f.header_json() for f in u.followings], 200
		else:
			return [],200

class FollowRest(Resource):
	"""
		One user follow another
	"""
	def post(self, user_id):
		user = check_authorization()
		try:
			target_user = User.query.filter(User.user_id==user_id).first()
			target_user.fans.append(user)
			db.session.add(user)
			db.session.commit()
			return {'result':'success'}, 200
		except Exception, e:
			return {'result':'fail'}, 404
		
class UnFollowRest(Resource):
	"""
		One user unfollow another user
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
		""" 
			Login restful api
		"""
		up = self.__user_parser()
		user = User.query.filter(User.facebook_token==up['facebooktoken']).first()
		if user is None:
			u = User(up['name'],up['description'],up['facebooktoken'])
			if u.validate():
				try:
					header = get('http://ww3.sinaimg.cn/mw690/63ea4d33gw1ejhpwui71sj20u00k045s.jpg').content #Test for get headers
					with store_context(fs_store):	
						u.header_icon.from_blob(header)
						db.session.add(u)
						flag = db.session.commit()
					u.token = make_token(u.user_id)
					db.session.add(u)
					db.session.commit()
					return {'token': u.token}, 200
				except Exception, e:
					return {'result':'fail'}, 500
			else:
				return {'result':'fail'}, 404
		else:
			check_expire(user.token)
			return {'token': user.token}, 200

	def __user_parser(self):
		up = reqparse.RequestParser()
		up.add_argument('facebooktoken', type=str, location='headers')
		up.add_argument('name', type=str, location='headers')
		up.add_argument('description', type=str, location='headers')
		up.add_argument('headerurl', type=str, location='headers')
		return up.parse_args()


class UserSearchRest(Resource):
	def get(self, user_name):
		u = check_authorization()
		if user_name is not None and user_name is not '':
			user_name = '%'+user_name+'%'
			users = User.query.filter(User.name.like(user_name)).all()
			if u in users:
				users.remove(u)
			return [user.header_json() for user in users], 200
		else:
			return [],200

