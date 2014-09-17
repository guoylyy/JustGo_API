# -*- coding: utf-8 -*-
import time as stime
import unittest
import random
import simplejson as json

from datetime import datetime
from app import create_app
from app import configs


ran_key = stime.mktime(datetime.now().timetuple())

user_name = 'tu_' + str(ran_key) + str(random.randint(1,1000))
facebook_token = 'fb_token_'+ str(ran_key) + str(random.randint(1,1000))

class BaseTest(unittest.TestCase):
    def setUp(self):
        self.app = create_app(configs.TestConfig,'test_app',True)
        self.app.config['TESTING'] = True
        self.client = self.app.test_client()       
        self.base_url = '/' + self.app.config['VERSION']
        self.token = self.login()
        self.user = self.get_user_profile(self.token)
        self.stranger = self.mk_user()

    def login(self):
        rep = self.client.post(self.base_url+'/login',data=dict(
            name= user_name,
            facebook_token= facebook_token,
            description='I am a test_user'
            ))
        return json.loads(rep.data)['token']

    def mk_user(self):
        rep = self.client.post(self.base_url+'/login',data=dict(
            name= user_name + '_t',
            facebook_token= facebook_token + '_t',
            description='I am a test user num 2'
            ))
        self.stranger_token = json.loads(rep.data)['token']
        return self.get_user_profile(json.loads(rep.data)['token'])

    def get_user_profile(self, token):
        rep = self.client.get(self.base_url+'/user/profile',
            headers={'Authorization' : token})
        return json.loads(rep.data)

    def tearDown(self):
        pass


class A_UserTest(BaseTest):
    def test_01_user_profile(self):
        print '\nStart to test User'

        print '===1-1 Test User Profile ==='
        rep = self.client.get(self.base_url+'/user/1')
        assert b'user_id' in rep.data
    
    def test_02_follow_and_unfollow(self):
        print '===1-2 Test Follow and Unfollow==='
        rep = self.client.post(self.base_url + '/user/follow/'+str(self.stranger['user_id']),
            headers={'Authorization':self.token})
        assert b'success' in rep.data

        print '==== Test fans'
        rep = self.client.get(self.base_url +'/user/fans', headers={'Authorization': self.stranger_token})
        assert b'header' in rep.data
        #print rep.data

        print '==== Test followings'
        rep = self.client.get(self.base_url + '/user/followings',  headers={'Authorization': self.token})
        assert b'header' in rep.data

        rep = self.client.post(self.base_url + '/user/unfollow/' + str(self.stranger['user_id']),
            headers={'Authorization':self.token})
        assert b'success' in rep.data 



class B_GoalTest(BaseTest):

    def test_01_get_category(self):
        print '\nStart to test Goal==>>'

        print '=== 2-1. Test get goal_category ==='
        rep = self.client.get(self.base_url+'/goal_category')
        assert b'category_name' in rep.data

    def test_02_get_goal_from_category(self):
        print '=== 2-2. Test get goal ==='
        rep = self.client.get(self.base_url+'/goal/Popular')
        #print rep.data
        assert b'goal_name' in rep.data

    def test_03_add_and_get_goal_record(self):
        print '=== 2-3. Add goal record ==='
        rep = self.client.post(self.base_url+'/goal_record',data=dict(
            goal_id=1,
            content='I like this goal',
            description='Really good'
            ),
            headers={'Authorization': self.token})
        assert b'goal_id' in rep.data

        goal_obj =json.loads(rep.data) 
        goal_id = goal_obj['goal_id']
        goal_record_id = goal_obj['goal_record_id']

        print '=== 2-4. Test get goal record by goal_record_id ==='
        rep = self.client.get(self.base_url+'/goal_record/'+str(goal_record_id),
            headers={'Authorization': self.token})
        assert b'goal_id' in rep.data
        #print rep.data

        print '=== 2-5. Test get all record for a user in a goal ===='
        rep = self.client.get(self.base_url+'/goal_join_record/'+str(goal_id),
            headers={'Authorization': self.token})
        assert b'goal_id' in rep.data
        #print rep.data


        print '=== 2-6. Awesome a record ===='
        rep = self.client.post(self.base_url+'/goal_record_awesome/'+str(goal_record_id),
            headers={'Authorization': self.token})
        assert b'awesome_id' in rep.data


        print '=== 2-7. Test get awesome ===='
        rep = self.client.get(self.base_url+'/goal_record_awesome/'+str(goal_record_id),
            headers={'Authorization': self.token})
        #print rep.data
        assert b'awesome_id' in rep.data


        print '=== 2-8. Test comment a record ===='
        rep = self.client.post(self.base_url+'/goal_record_comment/'+str(goal_record_id),
            data=dict(
                content='A test comment'
                ),
            headers={'Authorization': self.token})
        assert b'comment_id' in rep.data


        print '=== 2-9. Test get comments of a record ==='
        rep = self.client.get(self.base_url+'/goal_record_comment/'+str(goal_record_id))
        assert b'comment_id' in rep.data


        print '=== 2-10. Test List all Goal Record ==='
        rep = self.client.get(self.base_url+'/goal_record_list/'+str(goal_id),
             headers={'Authorization': self.token})
        #print rep.data

        print '=== 2-11. Test fighting center ==='
        rep = self.client.get(self.base_url+'/goal_record/fighting_center',
             headers={'Authorization': self.token})
        #print rep.data

    def test_sync(self):
        print '=== 2-10. Test Sync Goal Join ==='
        rep = self.client.get(self.base_url + '/sync_goal_join',
            headers={'Authorization':self.token})
        assert b'update_time' in rep.data
        update_time = json.loads(rep.data)['update_time']
        #print 'Last update time: ' + str(update_time)

        headers = {
            'Authorization' : self.token,
            'Content-Type' : 'application/json'
        }

        data = [
                {
                    "create_time": 1409820549,
                    "end_date": "2014-08-20",
                    "frequency": "1,2,3,4,5",
                    "goal_id": 1,
                    "goal_join_id": "goal_" + user_name,
                    "is_finished": False,
                    "is_reminder": True,
                    "reminder_time": "12:02:00",
                    "start_date": "2014-08-08",
                    "time_span": 7,
                    "update_time": 1409820549,
                    "user_id": self.user['user_id']
                }
            ]
        rep = self.client.post(self.base_url + '/sync_goal_join', headers=headers, data=json.dumps(data))
        assert b'success' in rep.data

        print '=== 2-11. Test Sync Goal Join Track ==='
        data = [
                {
                    "create_time": 1409846950,
                    "goal_join_id": "goal_" + user_name,
                    "goal_track_id": 1,
                    "track_date": "2014-08-09",
                    "update_time": 1409846950,
                    "user_id": self.user['user_id']
                }
            ]
        rep = self.client.post(self.base_url + '/sync_goal_join_track',
            headers=headers, data=json.dumps(data))
        assert b'success' in rep.data


class C_NotificationTest(BaseTest):
    def test_01_encourage(self):
        print '\nStart notification test ==>>'

        print '=== 3-1. Test encourage ==='
        rep = self.client.post(self.base_url + '/encourage/goal_' + user_name,
            headers={'Authorization' : self.stranger_token})
        assert b'content' in rep.data
        #print rep.data

        rep = self.client.get(self.base_url + '/notification',
            headers={'Authorization' : self.token})
        assert b'content' in rep.data
        #print rep.data

        rep = self.client.get(self.base_url + '/notification/mark_readed',
            headers={'Authorization' : self.token})
        assert b'success' in rep.data

        rep = self.client.get(self.base_url + '/notification',
            headers={'Authorization' : self.token})
        assert b'content' not in rep.data




    def test_02_mark_all_as_readed(self):
        
        pass

