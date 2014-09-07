from goal_api import *
from user_api import * 

def create_api(api, base_url):
	api.add_resource(UserRest, base_url + '/user/<int:user_id>', base_url + '/user')
	api.add_resource(LoginRest, base_url + '/login')
	api.add_resource(FollowRest, base_url + '/user/follow/<int:user_id>')
	api.add_resource(UnFollowRest, base_url + '/user/unfollow/<int:user_id>')
	api.add_resource(ProfileRest, base_url + '/user/profile')
	
	api.add_resource(GoalJoinTrackRest, base_url + '/goal_join_track')
	api.add_resource(GoalJoinRest, base_url + '/goal_join')
	api.add_resource(SyncGoalJoinRest, base_url + '/sync_goal_join', base_url + '/goal_join/update_time')
	api.add_resource(SyncGoalJoinTrackRest, base_url + '/sync_goal_join_track', base_url + '/goal_join_track/update_time')
	
	api.add_resource(GoalCategoryRest,  base_url + '/goal_category')
	api.add_resource(GoalRest,  base_url + '/goal/<string:category_id>')
	api.add_resource(GoalRecordRest,  base_url + '/goal_record/<int:record_id>', base_url + '/goal_record')
	api.add_resource(GoalRecordCommentRest,  base_url + '/goal_record_comment/<int:record_id>')
	api.add_resource(GoalRecordAwesomeRest,  base_url + '/goal_record_awesome/<int:record_id>')
	api.add_resource(GoalJoinRecordRest,  base_url + '/goal_join_record/<int:goal_id>')

	api.add_resource(NotificationRest,  base_url + '/notification')
	api.add_resource(EncourageRest,  base_url + '/encourage/<string:goal_join_id>')
	return api
