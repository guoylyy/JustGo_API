import os 
import random
import json
import csv
import time as stime
import hashlib
import simplejson as json
from datetime import datetime, timedelta, time , date
from flask import Flask, request, flash, url_for, redirect, \
	 render_template, abort, jsonify
from flask_sqlalchemy import SQLAlchemy
from sqlalchemy.orm import relationship, backref
from sqlalchemy.sql import or_
from sqlalchemy import Table 
from sqlalchemy.orm.exc import NoResultFound
from sqlalchemy_imageattach.entity import Image, image_attachment, store_context
from sqlalchemy_imageattach.stores.fs import HttpExposedFileSystemStore, FileSystemStore
from flask.ext import restful
from flask.ext.restful import fields, marshal, abort
from flask.ext.restful import reqparse

app = Flask(__name__)
app.config.from_pyfile('config.cfg')
db = SQLAlchemy(app)
api = restful.Api(app)

fs_store = FileSystemStore(app.config['IMAGE_UPLOAD_URL'], app.config['IMAGE_URL'])

parser = reqparse.RequestParser()
parser.add_argument('Authorization', type=str, location='headers')

#============= UDF ==================#
def abort_if_category_doesnt_exist(category_name):
	if Category.query.filter(Category.category_name==category_name).count() == 0:
		restful.abort(404, message="Category {} doesn't exist".format(category_name))

def abort_if_user_doesnt_exit(user_id):
	if User.query.filter(User.user_id==user_id).count() == 0:
		restful.abort(404, message="User with ID {} doesn't exist".format(user_id))
	else:
		return 	User.query.filter(User.user_id==user_id).first()

def check_authorization():
	token = parser.parse_args()['Authorization']
	if not token:
		restful.abort(500, message="Authorization Failed")
	else:
		# Check expired
		check_expire(token)
		u = User.query.filter(User.token==token).first()
		if u:
			return u
		else:
			restful.abort(500, message='Invalid Authorization')

#============= End of UDF ===========#


#============== APIs =============#
class UserRest(restful.Resource):
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


class LoginRest(restful.Resource):
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

class GoalCategoryRest(restful.Resource):
	def get(self):
		l = Category.query.all()
		return [c.to_json() for c in l], 200

class GoalRest(restful.Resource):
	# TO-DO: Add people number to each category
	def get(self, category_id):
		abort_if_category_doesnt_exist(category_id)
		c = Category.query.filter(Category.category_name==category_id).first()
		goals = c.goals
		return [g.to_json() for g in goals] ,200

	def __goal_details(self, goal_id):
		return ""

class GoalJoinRest(restful.Resource):
	def get(self):
		user = check_authorization()
		gjs = GoalJoin.query.filter(GoalJoin.user_id==user.user_id)
		return [gj.to_json() for gj in gjs]


class GoalJoinTrackRest(restful.Resource):
	def get(self):
		user = check_authorization()
		gjts = GoalTrack.query.filter(GoalTrack.user_id==user.user_id)
		return [gjt.to_json() for gjt in gjts]

class GoalJoinRecordRest(restful.Resource):
	""" API for Goal Join Record
	"""
	def get(self, goal_id):
		user = check_authorization()
		records = GoalRecord.query.filter(GoalRecord.goal_id==goal_id, GoalRecord.user_id == user.user_id)
		return [gr.to_json(user) for gr in records], 200


class GoalRecordRest(restful.Resource):
	def get(self, record_id):
		u = check_authorization()
		g = GoalRecord.query.filter(GoalRecord.goal_record_id==record_id).first()
		if g:
			g.comments = g.comments.all()
			g.awesomes = g.awesomes.all()
			return g.to_json(u), 200
		else:
			restful.abort(404, 'Goal Record with ID {} does exist.'.format(record_id))

	def post(self):
		""" Add a record for specific goal
		"""
		user = check_authorization()
		up = self.__record_parser()
		g = GoalRecord(up['goal_id'], user.user_id, up['content'], 'fdsafa')
		db.session.add(g)
		flag = db.session.commit()
		return g.to_json(user), 200

	def __record_parser(self):
		up = reqparse.RequestParser()
		up.add_argument('content', type=str, location='form')
		up.add_argument('goal_id', type=int, location='form')
		return up.parse_args()


class GoalRecordCommentRest(restful.Resource):
	def get(self, record_id):
		""" Get all comments of a goal_record
		"""
		gr = GoalRecord.query.filter(GoalRecord.goal_record_id==record_id).first()
		return [grc.to_json() for grc in gr.comments.all()], 200

	def post(self, record_id):
		""" Add a comment to specific goal_record
		"""
		user = check_authorization()
		up = self.__comment_parser()
		grc = GoalRecordComment(record_id, user.user_id, up['content'])
		db.session.add(grc)
		flag = db.session.commit()
		return {'result' : 'success'}, 200

	def __comment_parser(self):
		up = reqparse.RequestParser()
		up.add_argument('content', type=str, location='form')
		return up.parse_args()		


class GoalRecordAwesomeRest(restful.Resource):
	"""docstring for GoalRecordAwesomeRest"""
	def get(self, record_id):
		gr = GoalRecord.query.filter(GoalRecord.goal_record_id==record_id).first()
		return [gra.to_json() for gra in gr.awesomes.all()], 200

	def post(self, record_id):
		#TO-DO:forbid duplicate awesome operation
		user = check_authorization()
		gra = GoalRecordAwesome(record_id, user.user_id)
		db.session.add(gra)
		db.session.commit()
		gr = GoalRecord.query.filter(GoalRecord.goal_record_id==record_id).first()
		return [gra.to_json() for gra in gr.awesomes.all()], 200
		

class NotificationRest(restful.Resource):
	""" Methods to visit user's notification
	"""
	def get(self):
		""" Get user notifications
		"""
		user = check_authorization()
		notifications = Notification.query.filter(Notification.receiver_id==user.id, Notification.is_readed==False)
		return [n.to_json() for n in notifications]

class EncourageRest(restful.Resource):
	""" Send Encourage to someone's goal
	"""
	def post(self, goal_join_id):
		user = check_authorization()
		gj = GoalJoin.query.filter(GoalJoin.goal_join_id==goal_join_id).first()
		if gj:
			content = "%s encourage you to finish %s" % (user.name, gj.goal.goal_name)
			ntf = Notification('encourage', user.user_id, gj.user_id, content, gj.goal_id)
			db.session.add(ntf)
			db.session.commit()
		else:
			restful.abort(404, 'User %s didn\'t join this goal' % user.name)

class FollowRest(restful.Resource):
	"""
	"""
	def post(self, user_id):
		user = check_authorization()
		target_user = User.query.filter(User.user_id==user_id)
		target_user.fans.add(user)
		db.session.add(user)
		db.session.commit()
		return {'result':'success'}, 200

class UnFollowRest(restful.Resource):
	"""
	"""
	def post(self, user_id):
		user = check_authorization()
		target_user = User.query.filter(User.user_id==user_id)
		target_user.fans.remove(user)
		db.session.add(target_user)
		db.session.commit()
		return {'result':'success'}, 200


class SyncGoalJoinRest(restful.Resource):
	""" Sync GoalJoin and GoalJoinTrack
	"""

	def post(self):
		user = check_authorization()
		objs =  request.get_json(force=True)
		for gj in objs:
			gjd = GoalJoin.query.filter(GoalJoin.goal_join_id==gj['goal_join_id']).first()
			if gjd:
				gjd.update_from_json(**gj)
				db.session.add(gjd)
			else:
				gjo = GoalJoin(**gj)
				db.session.add(gjo)
		db.session.commit()
		return {'result':'success'} ,200

	def get(self):
		""" 
		"""
		user = check_authorization()
		gj = GoalJoin.query.filter(GoalJoin.user_id==user.user_id).order_by(GoalJoin.update_time).first()
		if gj:
			return {'update_time' : _mk_timestamp(gj.update_time) }, 200
		else:
			return {'update_time' : None}, 200

class SyncGoalJoinTrackRest(restful.Resource):
	""" Sync GoalJoin and GoalJoinTrack
	"""
	def post(self):
		user = check_authorization()
		objs =  request.get_json(force=True)
		for gjt in objs:
			s_gjt = GoalTrack.query.filter(GoalTrack.goal_join_id==gjt['goal_join_id'], \
				GoalTrack.track_date==_str2date(gjt['track_date'])).first()
			if s_gjt:
				s_gjt.update_from_json(**gjt)
				db.session.add(s_gjt)
			else:
				n_gjt = GoalTrack(**gjt)
				db.session.add(n_gjt)
		db.session.commit()
		return {'result':'success'} ,200

	def get(self):
		""" Get 
		"""
		user = check_authorization()
		gjt = GoalTrack.query.filter(GoalTrack.user_id==user.user_id).order_by(GoalTrack.update_time).first()
		if gjt:
			return {'update_time' : _mk_timestamp(gjt.update_time)}, 200
		else:
			return {'update_time' : None}, 200
#============== End of APIs =============#


#============== Routes ============#
BASE_URL = '/' + app.config['VERSION']

api.add_resource(GoalJoinTrackRest, BASE_URL + '/goal_join_track')
api.add_resource(GoalJoinRest, BASE_URL + '/goal_join')
api.add_resource(SyncGoalJoinRest, BASE_URL + '/sync_goal_join', BASE_URL + '/goal_join/update_time')
api.add_resource(SyncGoalJoinTrackRest, BASE_URL + '/sync_goal_join_track', BASE_URL + '/goal_join_track/update_time')

api.add_resource(LoginRest, BASE_URL + '/login')
api.add_resource(UserRest, BASE_URL + '/user/<int:user_id>', BASE_URL + '/user')
api.add_resource(FollowRest, BASE_URL + '/user/follow/<int:user_id>')
api.add_resource(UnFollowRest, BASE_URL + '/user/unfollow/<int:user_id>')

api.add_resource(GoalCategoryRest,  BASE_URL + '/goal_category')
api.add_resource(GoalRest,  BASE_URL + '/goal/<string:category_id>')
api.add_resource(GoalRecordRest,  BASE_URL + '/goal_record/<int:record_id>', BASE_URL + '/goal_record')
api.add_resource(GoalRecordCommentRest,  BASE_URL + '/goal_record_comment/<int:record_id>')
api.add_resource(GoalRecordAwesomeRest,  BASE_URL + '/goal_record_awesome/<int:record_id>')
api.add_resource(GoalJoinRecordRest,  BASE_URL + '/goal_join_record/<int:goal_id>')

api.add_resource(NotificationRest,  BASE_URL + '/notification')

#============== End of Routes =====#

#================== Models =====================#
user_follow = db.Table('user_follow',
	db.Column('follow_id', db.Integer, db.ForeignKey('user.user_id')),
	db.Column('followed_id', db.Integer, db.ForeignKey('user.user_id'))
)

class User(db.Model):
	__tablename__ = "user"
	user_id = db.Column(db.Integer, primary_key=True, autoincrement=True)
	name = db.Column(db.String)
	description = db.Column(db.String)
	token = db.Column(db.String)
	facebook_token = db.Column(db.String)
	header_icon = image_attachment('UserHeader')
	update_time = db.Column(db.DateTime)
	create_time = db.Column(db.DateTime)

	followings = db.relationship('User', secondary=user_follow, primaryjoin=user_id==user_follow.c.follow_id, \
		secondaryjoin=user_id==user_follow.c.followed_id,backref=db.backref('u_fans', lazy='dynamic'))
	
	fans = db.relationship('User', secondary=user_follow, primaryjoin=user_id==user_follow.c.followed_id, \
		secondaryjoin=user_id==user_follow.c.follow_id,backref=db.backref('u_followings', lazy='dynamic'))

	def __init__(self, name, description, token, facebook_token, header_icon):
		self.name = name
		self.description = description
		self.token = token
		self.facebook_token = facebook_token
		self.update_time = datetime.now()
		self.create_time = datetime.now()

	def header_json(self):
		return {
			'user_id' : self.user_id,
			'header' : _find_or_create_thumbnail(self, self.header_icon,48).locate(),
			'user_name' : self.name
		}

	def to_json(self):
		with store_context(fs_store):
			return {
				'user_id' : self.user_id,
				'name' : self.name,
				'description': self.description,
				'fans' : [f.header_json() for f in self.fans],
				'followings' : [fo.header_json() for fo in self.followings],
				'can_follow' : True,
				'header_icon' : _find_or_create_thumbnail(self, self.header_icon,48).locate()
			}

class UserHeader(db.Model, Image):
	__tablename__ = 'user_header'
	user_id = db.Column(db.Integer, db.ForeignKey('user.user_id'), primary_key=True)
	user = db.relationship('User')


goal_category = db.Table('goal_category',
	db.Column('goal_id', db.Integer, db.ForeignKey('goal.goal_id')),
	db.Column('category_name', db.String , db.ForeignKey('category.category_name'))
)

class Category(db.Model):
	""" GoalCategory
	"""
	__tablename__ = "category"
	category_name = db.Column(db.String, primary_key=True)
	desciprtion = db.Column(db.String)
	create_time = db.Column(db.DateTime)
	update_time = db.Column(db.DateTime)
	goals = db.relationship('Goal', secondary=goal_category, backref=db.backref('categorys', lazy='dynamic'))

	def __init__(self, category_name, update_time, desciprtion):
		self.category_name = category_name
		self.desciprtion = desciprtion
		self.update_time = update_time
		self.create_time = datetime.now()

	def to_json(self):
		return {
			'category_name' : self.category_name,
			'desciprtion' : self.desciprtion
		}

class Goal(db.Model):
	""" Goal table is only write in server """
	__tablename__ = "goal"
	goal_id = db.Column(db.Integer, primary_key=True, autoincrement=True)
	goal_name = db.Column(db.String, unique=True)
	image = image_attachment('GoalImage')
	desciprtion = db.Column(db.String)
	create_time = db.Column(db.DateTime)
	update_time = db.Column(db.DateTime)
	goal_joins = db.relationship('GoalJoin', backref='goal', lazy='dynamic')

	def __init__(self, goal_name, description):
		self.goal_name = goal_name
		self.description = description
		self.create_time = datetime.now()
		self.update_time = datetime.now()

	def to_json(self):
		with store_context(fs_store):
			return {
				'goal_id' : self.goal_id,
				'goal_name' : self.goal_name,
				'desciprtion' : self.desciprtion,
				'joins' : self.goal_joins.count(),
				'image' : self.image.locate()
			}

class GoalImage(db.Model, Image):
	__tablename__ = 'goal_image'
	user_id = db.Column(db.Integer, db.ForeignKey('goal.goal_id'), primary_key=True)
	user = db.relationship('Goal')

class GoalJoin(db.Model):
	__tablename__ = "goal_join"
	goal_join_id = db.Column(db.String, primary_key=True)
	goal_id = db.Column(db.Integer, db.ForeignKey('goal.goal_id'))
	user_id = db.Column(db.Integer, db.ForeignKey('user.user_id'))
	time_span = db.Column(db.Integer)
	frequency = db.Column(db.String)
	is_reminder = db.Column(db.Boolean)
	reminder_time = db.Column(db.Time)
	start_date = db.Column(db.Date)
	end_date = db.Column(db.Date)
	is_finished = db.Column(db.Boolean, default=False)
	update_time = db.Column(db.DateTime)
	create_time = db.Column(db.DateTime)
	
	def __init__(self, *args , **kwargs):
		if args:
			self.goal_join_id = args[0]
			self.goal_id = args[1]
			self.user_id = args[2]
			self.time_span = args[3]
			self.frequency = args[4]
			self.is_reminder = args[5]
			self.reminder_time = args[6]
			self.start_date = args[7]
			self.end_date = args[8]
			self.is_finished = args[9]
			self.update_time = self.create_time = datetime.now()
		else:
			self.goal_join_id = kwargs['goal_join_id']
			self.__receive_from_json(kwargs)


	def update_from_json(self, **kwargs):
		self.__receive_from_json(kwargs)

	def __receive_from_json(self, kwargs):
		self.goal_id = kwargs['goal_id']
		self.user_id = kwargs['user_id']
		self.time_span = kwargs['time_span']
		self.frequency = kwargs['frequency']
		self.is_reminder = kwargs['is_reminder']
		self.reminder_time = _str2time(kwargs['reminder_time'])
		self.start_date = _str2date(kwargs['start_date'])
		self.end_date = _str2date(kwargs['end_date'])
		self.is_finished = kwargs['is_finished']
		self.update_time = datetime.now()
		self.create_time = datetime.fromtimestamp(kwargs['create_time'])

	def to_json(self):
		return {
			'goal_join_id' : self.goal_join_id,
			'goal_id' : self.goal_id,
			'user_id' : self.user_id,
			'time_span': self.time_span,
			'frequency' : self.frequency,
			'is_reminder' : self.is_reminder,
			'reminder_time' : self.reminder_time.isoformat(),
			'start_date' : self.start_date.isoformat(),
			'end_date' : self.end_date.isoformat(),
			'is_finished': self.is_finished,
			'update_time' : _mk_timestamp(self.update_time),
			'create_time' : _mk_timestamp(self.create_time)
		}

	def __get_goal(self):
		return Goal.query.filter(Goal.goal_id==self.goal_id).first()

	goal = property(__get_goal)

class GoalTrack(db.Model):
	""" Track everydays' status of a goal for one user """
	__tablename__ = "goal_track"
	goal_track_id = db.Column(db.Integer, autoincrement=True, primary_key=True)
	goal_join_id = db.Column(db.Integer, db.ForeignKey('goal_join.goal_join_id'))
	user_id = db.Column(db.Integer, db.ForeignKey('user.user_id'))
	track_date = db.Column(db.Date)
	update_time = db.Column(db.DateTime)
	create_time = db.Column(db.DateTime)

	def __init__(self, *args, **kwargs):
		if args:
			self.goal_join_id = args[0]
			self.user_id = args[1]
			self.track_date = args[2]
			self.update_time = self.create_time = datetime.now()
		else:
			self.__receive_from_json(kwargs)

	def update_from_json(self, **kwargs):
		self.__receive_from_json(kwargs)

	def __receive_from_json(self, kwargs):
		self.goal_join_id = kwargs['goal_join_id']
		self.user_id = kwargs['user_id']
		self.track_date = _str2date(kwargs['track_date'])
		self.update_time = datetime.now()

	def to_json(self):
		return {
			'goal_track_id' : self.goal_track_id,
			'goal_join_id' : self.goal_join_id,
			'user_id' : self.user_id,
			'track_date' : self.track_date.isoformat(),
			'update_time' : _mk_timestamp(self.update_time),
			'create_time' : _mk_timestamp(self.create_time)
		}

class GoalRecord(db.Model):
	""" Records for a specific goal """
	__tablename__ = 'goal_record'
	goal_record_id = db.Column(db.Integer, primary_key=True, autoincrement=True)
	goal_id = db.Column(db.Integer, db.ForeignKey('goal.goal_id'))
	user_id = db.Column(db.Integer, db.ForeignKey('user.user_id'))
	content = db.Column(db.String)
	image = db.Column(db.String)
	create_time = db.Column(db.DateTime)
	update_time = db.Column(db.DateTime)
	comments = db.relationship('GoalRecordComment', backref='goal_record', lazy='dynamic')
	awesomes = db.relationship('GoalRecordAwesome', backref='goal_record', lazy='dynamic')

	def __init__(self, goal_id, user_id, content, image):
		self.goal_id = goal_id
		self.user_id = user_id
		self.content = content
		self.image = image
		self.update_time = datetime.now()
		self.create_time = datetime.now()

	def __can_awesome(self, user):
		if (user.user_id == self.user_id) or (user.user_id in [ga.user_id for ga in self.awesomes.all()]):
			return False
		else:
			return True

	def to_json(self, user):
		return {
			'goal_record_id': self.goal_record_id,
			'goal_id' : self.goal_id,
			'content' : self.content,
			'image' : self.image,
			'comments' : [c.to_json() for c in self.comments.all()],
			'awesomes' : [a.to_json() for a in self.awesomes.all()],
			'can_awesome' : self.__can_awesome(user)
		}

class GoalRecordComment(db.Model):
	"""docstring for GoalRecordComment"""
	__tablename__ = 'goal_record_comment'
	comment_id = db.Column(db.Integer, primary_key=True, autoincrement=True)
	goal_record_id = db.Column(db.Integer, db.ForeignKey('goal_record.goal_record_id'))
	user_id = db.Column(db.Integer, db.ForeignKey('user.user_id'))
	content = db.Column(db.String)
	update_time = db.Column(db.DateTime)
	create_time = db.Column(db.DateTime)

	def __init__(self, goal_record_id, user_id, content):
		self.goal_record_id = goal_record_id
		self.user_id = user_id
		self.content = content
		self.update_time = datetime.now()
		self.create_time = datetime.now()

	def to_json(self):
		return {
			'comment_id' : self.comment_id,
			'goal_record_id' : self.goal_record_id,
			'user_id' : self.user_id,
			'content' : self.content,
			'create_time' : time.mktime(self.create_time.timetuple())
		}


class GoalRecordAwesome(db.Model):
	"""docstring for GoalRecordAwesome"""
	__tablename__ = 'goal_record_awesome'
	awesome_id = db.Column(db.Integer, primary_key=True, autoincrement=True)
	goal_record_id = db.Column(db.Integer, db.ForeignKey('goal_record.goal_record_id'))
	user_id = db.Column(db.Integer, db.ForeignKey('user.user_id'))
	update_time = db.Column(db.DateTime)
	create_time = db.Column(db.DateTime)
	
	def __init__(self, goal_record_id, user_id):
		self.goal_record_id = goal_record_id
		self.user_id = user_id
		self.create_time = datetime.now()
		self.update_time = datetime.now()

	def to_json(self):
		return {
			'awesome_id' : self.awesome_id,
			'goal_record_id' : self.goal_record_id,
			'user_id' : self.user_id
		}

class Notification(db.Model):
	"""docstring for Notification
	"""
	__tablename__ = 'notification'
	notificaion_id = db.Column(db.Integer, primary_key=True, autoincrement=True)
	notificaion_type = db.Column(db.String)
	sender_id = db.Column(db.Integer, db.ForeignKey('user.user_id'))
	receiver_id = db.Column(db.Integer, db.ForeignKey('user.user_id'))
	content = db.Column(db.String)
	attach_key = db.Column(db.String)
	update_time = db.Column(db.DateTime)
	create_time = db.Column(db.DateTime)
	is_readed = db.Column(db.Boolean, default=False)
	
	def __init__(self, notificaion_type, sender_id, receiver_id, content, attach_key):
		self.notificaion_type = notificaion_type
		self.sender_id = sender_id
		self.receiver_id = receiver_id
		self.content = content
		self.attach_key = str(attach_key)
		self.update_time = self.create_time = datetime.now()

	def to_json(self):
		return {
			'notificaion_id' : self.notificaion_id,
			'user_id' : self.user_id,
			'content' : self.content
		}


#============== End of Models ============================#

#============== Query ===============#


#============== End Query ===========#

def make_token(id):
    expire_day = app.config['SESSION_EXPIRE_DAYS']
    expire_time = (datetime.now() + timedelta(expire_day,0)).timetuple();
    expire_datetime = stime.mktime(expire_time)
    expire = str(int(expire_datetime))
    s = '%s:%s:%s' % (id, expire, app.config['MD5_RANDOM'])
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

def init_db():
	db.drop_all()
	db.create_all()
	make_wp_data()

def update_db():
	db.update()

def make_wp_data():
	categories = ['Popular', 'Health Diet', 'Train plans', 'Habits', 'Learning']
	for cat in categories:
		db.session.add(Category(cat, datetime.now(), ''))
	goals = [
			['Drink water','description','Popular'],
			['Make love Every Day','description','Popular'],
			['Write article ','description','Popular'],
			['Do fitness','description','Health Diet'],
			['Watch Movie','description','Train plans']
		]
	db.session.commit()
	c = Category.query.filter(Category.category_name=='Popular').first()

	g_list = []
	with store_context(fs_store):
		for g in goals:
			goal = Goal(g[0], g[1])
			with open('pic1.jpg','rb') as f:
				goal.image.from_blob(f.read())
			g_list.append(goal)
			db.session.add(goal)
		c.goals = g_list
		db.session.add(c)
		db.session.commit()

def _str2date(dstr):
	ym = [int(d) for d in dstr.split('-')]
	return date(ym[0],ym[1],ym[2])

def _str2time(tstr):
	tm = [int(t) for t in tstr.split(':')]
	return time(tm[0],tm[1])

def _mk_timestamp(datetime):
	return stime.mktime(datetime.timetuple())

def _test_categories():
	c = Category.query.filter(Category.category_name=='Popular').count()
	print c

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

	
def make_test_goal_join():
	gj = GoalJoin('fs332ab',1,1,7,'1,2,3,4,5',True,time(12,2), date(2014,8,8),date(2014,8,20), False)
	gt1 = GoalTrack('fs332ab',1, date(2014,8,9))
	gt2 = GoalTrack('fs332ab',1, date(2014,8,10))
	db.session.add(gj)
	db.session.add(gt1)
	db.session.add(gt2)
	db.session.commit()

def user_test():
	u = User.query.filter(User.user_id==1).first()
	print u.user_id
	with store_context(fs_store):
		tb = _find_or_create_thumbnail(u, u.header_icon, 55)
		print tb.locate()
		print u.header_icon.locate()

if __name__ == '__main__':
	#init_db()
	#make_test_goal_join()
	#user_test()
	app.run(host='0.0.0.0',port=5000)
