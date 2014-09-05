from datetime import datetime
from sqlalchemy_imageattach.entity import Image, image_attachment, store_context
from flask.ext.restful import Resource, reqparse
from app.helpers import * 
from app.extensions import db, fs_store

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

#============== End of APIs =============#