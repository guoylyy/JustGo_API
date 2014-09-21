from flask import request
from datetime import datetime
from sqlalchemy_imageattach.entity import Image, image_attachment, store_context
from flask.ext.restful import Resource, reqparse, abort
from app.helpers import _str2date 
from app.helpers import _mk_timestamp
from app.extensions import db, fs_store
from app.models import *
from api_helpers import abort_if_category_doesnt_exist
from api_helpers import check_authorization , internal_error, parameter_error
 
class GoalCategoryRest(Resource):
	def get(self):
		l = Category.query.all()
		return [c.to_json() for c in l], 200

class GoalRest(Resource):
	def get(self, category_id):
		abort_if_category_doesnt_exist(category_id)
		c = Category.query.filter(Category.category_name==category_id, Goal.is_active==True).first()
		goals = c.goals
		return [g.to_json() for g in goals], 200

class GoalDetails(Resource):
	def get(self, goal_id):
		g = Goal.query.filter(Goal.goal_id==goal_id).first()
		if g:
			return g.to_details_json(),200
		else:
			abort(404, message='Goal not found')

class GoalJoinRest(Resource):
	def get(self):
		user = check_authorization()
		gjs = GoalJoin.query.filter(GoalJoin.user_id==user.user_id)
		return [gj.to_json() for gj in gjs], 200

class GoalJoinTrackRest(Resource):
	def get(self):
		user = check_authorization()
		gjts = GoalTrack.query.filter(GoalTrack.user_id==user.user_id)
		return [gjt.to_json() for gjt in gjts], 200

class GoalJoinRecordRest(Resource):
	def get(self, goal_id):
		""" 
			Get records for a user in specific goal
		"""
		user = check_authorization()
		records = GoalRecord.query.filter(GoalRecord.goal_id==goal_id, GoalRecord.user_id == user.user_id)
		return [gr.to_preview_json(user) for gr in records], 200

class UserGoalRecordRest(Resource):
	def get(self):
		"""
			Get all goal_record made by user
		"""
		user = check_authorization()
		records = GoalRecord.query.filter(GoalRecord.user_id == user.user_id)
		return [gr.to_preview_json(user) for gr in records], 200

class OtherUserGoalRecordRest(Resource):
	def get(self, user_id):
		user = User.query.filter(User.user_id==user_id).first()
		records = GoalRecord.query.filter(GoalRecord.user_id == user.user_id)
		return [gr.to_preview_json(user) for gr in records], 200

class GoalRecordRest(Resource):
	def get(self, record_id):
		"""
			Get one record by specific record_id
		"""
		u = check_authorization()
		g = GoalRecord.query.filter(GoalRecord.goal_record_id==record_id).first()
		if g:
			g.comments = g.comments.all()
			g.awesomes = g.awesomes.all()
			return g.to_json(u), 200
		else:
			abort(404, message='Goal Record with ID {} does exist.'.format(record_id))

	def post(self):
		""" 
			Add a record for specific goal
		"""
		user = check_authorization()
		up = self.__record_parser()
		g = GoalRecord(up['goalid'], user.user_id, up['content'])
		db.session.add(g)
		flag = db.session.commit()
		return g.to_json(user), 200

	def __record_parser(self):
		up = reqparse.RequestParser()
		up.add_argument('content', type=str, location='headers')
		up.add_argument('goalid', type=int, location='headers')
		return up.parse_args()

class GoalRecordListRest(Resource):
	def get(self, goal_id):
		"""
			Get all goal records by specific goal_id
		"""
		user = check_authorization()
		grs = GoalRecord.query.filter(GoalRecord.goal_id==goal_id).all()
		if grs:
			return [gr.to_preview_json(user) for gr in grs], 200
		else:
			return [],200

class GoalRecordCommentRest(Resource):
	def get(self, record_id):
		""" 
			Get all comments of a goal_record
		"""
		gr = GoalRecord.query.filter(GoalRecord.goal_record_id==record_id).first()
		return [grc.to_json() for grc in gr.comments.all()], 200

	def post(self, record_id):
		""" 
			Add a comment to specific goal_record
		"""
		user = check_authorization()
		up = self.__comment_parser()
		grc = GoalRecordComment(record_id, user.user_id, up['content'])
		db.session.add(grc)
		flag = db.session.commit()
		return grc.to_json(), 200

	def __comment_parser(self):
		up = reqparse.RequestParser()
		up.add_argument('content', type=str, location='headers')
		return up.parse_args()		


class GoalRecordAwesomeRest(Resource):
	def get(self, record_id):
		""" 
			Get all record awesomes for one record
		"""
		gr = GoalRecord.query.filter(GoalRecord.goal_record_id==record_id).first()
		return [gra.to_json() for gra in gr.awesomes.all()], 200

	def post(self, record_id):
		"""
			Awesome a record
		"""
		user = check_authorization()
		tg = GoalRecordAwesome.query.filter(GoalRecordAwesome.goal_record_id==record_id,
			GoalRecordAwesome.user_id == user.user_id).first()
		if tg:
			return tg.to_json(), 200
		else:
			gra = GoalRecordAwesome(record_id, user.user_id)
			db.session.add(gra)
			db.session.commit()
			return gra.to_json(), 200

class SyncGoalJoinRest(Resource):
	def post(self):
		""" 
			Sync GoalJoin and GoalJoinTrack
		"""
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
			Get lastest update time
		"""
		user = check_authorization()
		gj = GoalJoin.query.filter(GoalJoin.user_id==user.user_id).order_by(GoalJoin.update_time).first()
		if gj:
			return {'update_time' : _mk_timestamp(gj.update_time) }, 200
		else:
			return {'update_time' : None}, 200

class SyncGoalJoinTrackRest(Resource):
	def post(self):
		""" 
			Sync GoalJoin and GoalJoinTrack
		"""
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
		""" 
			Get lastest update time
		"""
		user = check_authorization()
		gjt = GoalTrack.query.filter(GoalTrack.user_id==user.user_id).order_by(GoalTrack.update_time).first()
		if gjt:
			return {'update_time' : _mk_timestamp(gjt.update_time)}, 200
		else:
			return {'update_time' : None}, 200

class ExploreRest(Resource):
	def get(self):
		key_category = 'Popular'
		c = Category.query.filter(Category.category_name==key_category).first()
		if c:
			return [g.to_json() for g in c.goals]
		else:
			abort(404, message='Explore not found')

class NotificationRest(Resource):
	def get(self):
		""" 
			Get user notifications
		"""
		user = check_authorization()
		notifications = Notification.query.filter(Notification.receiver_id==user.user_id,\
			 Notification.is_readed==False)
		return [n.to_json() for n in notifications], 200

class MarkNotficationReadRest(Resource):
	def get(self):
		"""
			Mark all notification as readed
		"""
		user = check_authorization()
		db.session.query(Notification).filter(Notification.receiver_id==user.user_id).update({'is_readed':True})
		db.session.commit()
		return {'result' : 'success'}, 200


class EncourageRest(Resource):
	def post(self, user_id):
		""" 
			Send Encourage to someone's goal
		"""
		user = check_authorization()
		receiver = User.query.filter(User.user_id==user_id).first()
		b_now = datetime.fromtimestamp(stime.mktime(date.today().timetuple()))
		n = Notification.query.filter(Notification.sender_id==user.user_id, \
			Notification.create_time>=b_now).first()
		if n or receiver is None:
			abort(500, message="You have encouraged this user today")
		else:
			content = "%s encourage you to finish goal." % (user.name)
			ntf = Notification('encourage', user.user_id, receiver.user_id, content, user.user_id)
			db.session.add(ntf)
			db.session.commit()
			return {'result' : 'success'}, 200
#============== End of APIs =============#