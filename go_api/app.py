import os 
import random
import json
import csv
import time
import hashlib
import simplejson as json
from datetime import datetime,timedelta
from flask import Flask, request, flash, url_for, redirect, \
	 render_template, abort, jsonify
from flask_sqlalchemy import SQLAlchemy
from sqlalchemy.orm import relationship, backref
from sqlalchemy.sql import or_
from sqlalchemy import Table 
from flask.ext import restful
from flask.ext.restful import fields, marshal, abort
from flask.ext.restful import reqparse

app = Flask(__name__)
app.config.from_pyfile('config.cfg')
db = SQLAlchemy(app)
api = restful.Api(app)

parser = reqparse.RequestParser()
parser.add_argument('Authorization', type=str, location='headers')

#============== Decorators =============#

#=============  End Decorators =========#


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

class GoalJoinRecord(restful.Resource):
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
		

#============== End of APIs =============#


#============== Routes ============#
api.add_resource(LoginRest,'/'+ app.config['VERSION'] + '/login')
api.add_resource(UserRest,'/'+ app.config['VERSION'] + '/user/<int:user_id>','/'+ app.config['VERSION'] + '/user')

api.add_resource(GoalCategoryRest, '/'+ app.config['VERSION'] + '/goal_category')
api.add_resource(GoalRest, '/'+ app.config['VERSION'] + '/goal/<string:category_id>')
api.add_resource(GoalRecordRest, '/'+ app.config['VERSION'] + '/goal_record/<int:record_id>','/'+ app.config['VERSION'] + '/goal_record')
api.add_resource(GoalRecordCommentRest, '/'+ app.config['VERSION'] + '/goal_record_comment/<int:record_id>')
api.add_resource(GoalRecordAwesomeRest, '/'+ app.config['VERSION'] + '/goal_record_awesome/<int:record_id>')
api.add_resource(GoalJoinRecord, '/'+ app.config['VERSION'] + '/goal_join_record/<int:goal_id>')


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
	update_time = db.Column(db.DateTime)
	token = db.Column(db.String)
	facebook_token = db.Column(db.String)
	header_icon = db.Column(db.String)

	followings = db.relationship('User', secondary=user_follow, primaryjoin=user_id==user_follow.c.follow_id, \
		secondaryjoin=user_id==user_follow.c.followed_id,backref=db.backref('u_fans', lazy='dynamic'))
	
	fans = db.relationship('User', secondary=user_follow, primaryjoin=user_id==user_follow.c.followed_id, \
		secondaryjoin=user_id==user_follow.c.follow_id,backref=db.backref('u_followings', lazy='dynamic'))

	def __init__(self, name, description, token, facebook_token, header_icon):
		self.name = name
		self.description = description
		self.token = token
		self.facebook_token = facebook_token
		self.header_icon = header_icon
		self.update_time = datetime.now()

	def header_json(self):
		return {
			'user_id' : self.user_id,
			'header' : 'fdsafasf',
			'user_name' : self.name
		}

	def to_json(self):
		return {
			'user_id' : self.user_id,
			'name' : self.name,
			'description': self.description,
			'fans' : [f.header_json() for f in self.fans],
			'followings' : [fo.header_json() for fo in self.followings],
			'can_follow' : True
		}

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
	update_time = db.Column(db.DateTime)
	goals = db.relationship('Goal', secondary=goal_category, backref=db.backref('categorys', lazy='dynamic'))

	def __init__(self, category_name, update_time, desciprtion):
		self.category_name = category_name
		self.desciprtion = desciprtion
		self.update_time = update_time

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
	image = db.Column(db.String)
	desciprtion = db.Column(db.String)
	update_time = db.Column(db.DateTime)
	goal_joins = db.relationship('GoalJoin', backref='goal', lazy='dynamic')

	def to_json(self):
		return {
			'goal_id' : self.goal_id,
			'goal_name' : self.goal_name,
			'desciprtion' : self.desciprtion,
			'joins' : self.goal_joins.count()
		}
	
class GoalJoin(db.Model):
	__tablename__ = "goal_join"
	goal_join_id = db.Column(db.String, primary_key=True)
	goal_id = db.Column(db.Integer, db.ForeignKey('goal.goal_id'))
	user_id = db.Column(db.Integer)
	time_span = db.Column(db.Integer)
	frequency = db.Column(db.String)
	is_reminder = db.Column(db.Boolean)
	reminder_time = db.Column(db.Time)
	start_date = db.Column(db.DateTime)
	end_date = db.Column(db.DateTime)
	is_finished = db.Column(db.Boolean)
	update_time = db.Column(db.DateTime)


class GoalTrack(db.Model):
	""" Track everydays' status of a goal for one user """
	__tablename__ = "goal_track"
	goal_track_id = db.Column(db.Integer, autoincrement=True, primary_key=True)
	goal_join_id = db.Column(db.Integer, db.ForeignKey('goal_join.goal_join_id'))
	track_date = db.Column(db.DateTime)
	update_time = db.Column(db.DateTime)


class GoalRecord(db.Model):
	""" Records for a specific goal """
	__tablename__ = 'goal_record'
	goal_record_id = db.Column(db.Integer, primary_key=True, autoincrement=True)
	goal_id = db.Column(db.Integer, db.ForeignKey('goal.goal_id'))
	user_id = db.Column(db.Integer, db.ForeignKey('user.user_id'))
	content = db.Column(db.String)
	image = db.Column(db.String)
	update_time = db.Column(db.DateTime)
	comments = db.relationship('GoalRecordComment', backref='goal_record', lazy='dynamic')
	awesomes = db.relationship('GoalRecordAwesome', backref='goal_record', lazy='dynamic')

	def __init__(self, goal_id, user_id, content, image):
		self.goal_id = goal_id
		self.user_id = user_id
		self.content = content
		self.image = image

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
	update_time = db.Column(db.DateTime)
	create_time = db.Column(db.DateTime)
	is_readed = db.Column(db.Boolean, default=False)
	
	def __init__(self, notificaion_type, sender_id, receiver_id, content):
		self.notificaion_type = notificaion_type
		self.sender_id = sender_id
		self.receiver_id = receiver_id
		self.content = content
		self.update_time = self.create_time = datetime.now()

	def to_json(self):
		return {
			'notificaion_id' : self.notificaion_id,
			'user_id' : self.user_id,
			'content' : self.content
		}

#============== End of Models ============================#

def make_token(id):
    expire_day = app.config['SESSION_EXPIRE_DAYS']
    expire_time = (datetime.now() + timedelta(expire_day,0)).timetuple();
    expire_datetime = time.mktime(expire_time)
    expire = str(int(expire_datetime))
    s = '%s:%s:%s' % (id, expire, app.config['MD5_RANDOM'])
    md5 = hashlib.md5(s.encode('utf-8')).hexdigest()
    return md5+":"+expire+":"+str(id)

def check_expire(token):
    '''
    return 
    - True for expire token
    - False for valid token
    - None for error token
    '''
    try :
        md5, expire, id = token.split(":", 3)
        if int(expire) > time.time():
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
	db.session.commit()

def _test_categories():
	c = Category.query.filter(Category.category_name=='Popular').count()
	print c

def make_goal_data():
	pass

def user_test():
	u = User.query.filter(User.user_id==1).first()
	print u.user_id
	print u.fans

if __name__ == '__main__':
	#user_test()
	app.run(host='0.0.0.0',port=5000)
