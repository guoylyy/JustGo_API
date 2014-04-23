from django.test import TestCase, Client
from goal.models import *
from user.models import *
import simplejson

# Create your tests here.

class GoalTest(TestCase):
    c = Client()

    def setUp(self):
        # insert new user
        f = UploadFile(file_name="test")
        f.save()
        u = User(name='test', password='test', email='test@test.com', portrait=f)
        u.save()

        self.user1 = u

        u2 = User(name='test2', password='test2', email='test2@test2.com', portrait=f)
        u2.save();
        self.user2 = u

        u3 = User(name='test2', password='test2', email='test2@test2.com', portrait=f)
        u3.save();
        self.user3 = u

        self.user_id = str(u.id)
        self.md5 = 'fca29f6feeb27cba36bd4b24a6a0f2e2'
        self.expire_time = '1400721018'
        self.token = self.md5 + ":" + self.expire_time + ":" + self.user_id

        # insert new token
        t = UserToken(user_id=u, token=self.token)
        t.save()

        # insert two category
        self.c1 = Category(name="test_category_1")
        self.c1.save()
        self.c2 = Category(name="test_category_2")
        self.c2.save()

        self.g1 = Goal(name='goal1', category=self.c1, detail='goal1_detail')
        self.g1.save()
        self.g2 = Goal(name='goal2', category=self.c1, detail='goal2_detail')
        self.g2.save()
        self.g3 = Goal(name='goal3', category=self.c2, detail='goal3_detail')
        self.g3.save()
        self.g4 = Goal(name='goal4', category=self.c2, detail='goal4_detail')
        self.g4.save()

        # insert joined goal
        self.ug1 = UserGoal(user=u, goal=self.g1, privacy='private')
        self.ug1.save()

        self.ug2 = UserGoal(user=u, goal=self.g2, privacy='public')
        self.ug2.save()

        # insert user_goal_checkout
        self.ugc1 = UserGoalCheckout(user_goal=self.ug1, comment="test")
        self.ugc1.save()
        self.ugc2 = UserGoalCheckout(user_goal=self.ug1, comment="test")
        self.ugc2.save()
        self.ugc3 = UserGoalCheckout(user_goal=self.ug2, comment="test")
        self.ugc3.save()
        self.ugc4 = UserGoalCheckout(user_goal=self.ug2, comment="test")
        self.ugc4.save()

    # test make_comment
    def test_make_comment(self):
        print ('\ntest make_comment')
        response = self.c.post('/goal/make_comment', {'token': self.token, 'checkout_id':self.ugc1.id, 'comment':'test'})
        print (response.content)

        ugc = UserGoalComment.objects.all().values_list('comment')
        print (ugc)

    # test make_awesome
    def test_make_awesome(self):
        print ('\ntest make_awesome')
        response = self.c.post('/goal/make_awesome', {'token':self.token, 'checkout_id':self.ugc2.id})
        print (response.content)

        uga = UserGoalAwesome.objects.all().values_list('checkout')
        print (uga)

    # test sync_pull
    def test_sync_pull(self):
        print ('\ntest sync_pull')
        response = self.c.get('/goal/sync_pull', {'token': self.token})
        print (simplejson.loads(response.content))

    # test sync_push
    def test_sync_push(self):
        print ('\ntest sync_push')
        json = '''
        [{
            "goal_id":"%d",
            "check_date":"1400721018", 
            "comment":"test"
        }, {
            "goal_id":"%d",
            "check_date":"1400721018",
            "comment":"test"
        }, {
            "goal_id":"%d",
            "check_date":"1400721018",
            "comment":"test"
        }, {
            "goal_id":"%d",
            "check_date":"1400721018",
            "comment":"test"
        }]''' % (self.g1.id, self.g1.id, self.g2.id, self.g2.id)
        response = self.c.post('/goal/sync_push', {'token':self.token, 'data':json})
        print (response.content)

        check_rec = UserGoalCheckout.objects.all().values_list('comment', 'date_check')
        print(check_rec)

    # test get_all_category
    def test_get_all_category(self):
        print ('\ntest get_all_category')
        response = self.c.get('/goal/category', {'token':self.token})
        print (response.content)

    # test get_goal_list
    def test_get_goal_list(self):
        print ('\ntest get_goal_list')
        response = self.c.get('/goal/goal_list', {'token':self.token, 'category_id':self.c1.id})
        print (response.content)

        response = self.c.get('/goal/goal_list', {'token':self.token, 'category_id':self.c2.id})
        print (response.content)

    # test get_goal_list_all
    def test_get_goal_list_all(self):
        print ('\ntest get_goal_list_all')
        response = self.c.get('/goal/goal_list_all', {'token':self.token})
        print (response.content)

    # test join_goal
    def test_join_goal(self):
        print ('\ntest join goal')
        response = self.c.post('/goal/join', {'token':self.token, 'goal_id':self.g1.id, 'privacy':'private'})
        print (response.content)
        user_goal = UserGoal.objects.all().values_list('user', 'goal', 'privacy')
        print (user_goal)

    # test exit_goal
    def test_exit_goal(self):
        print ('\ntest exit_goal')
        response = self.c.get('/goal/exit', {'token':self.token, 'goal_id':self.g1.id})
        print (response.content)