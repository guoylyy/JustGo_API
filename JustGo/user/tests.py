from django.test import TestCase, Client
from user.models import * 
from uploadfile.models import *

# Create your tests here.

class UserTest(TestCase):
    c = Client()

    def setUp(self):
        # insert new user
        f = UploadFile(file_name="test")
        f.save()
        u = User(name='test', password='test', email='test@test.com', portrait=f)
        u.save()

        self.user_id = str(u.id)
        self.md5 = 'fca29f6feeb27cba36bd4b24a6a0f2e2'
        self.expire_time = '1400721018'
        self.token = self.md5 + ":" + self.expire_time + ":" + self.user_id

        # insert new token
        t = UserToken(user_id=u, token=self.token)
        t.save()

    # test register
    def test_register(self):
        print("\ntest register user")
        response = self.c.post('/user/register',{'username':'admin','password':'admin','email':'123@qq.com'})
        print(response.content)

    # test login
    def test_login(self):
        print ("\ntest login")
        response = self.c.post('/user/login', {'username':'test','password':'test'})
        print (response.content)	

    # test token
    def test_token(self):
        print ('\ntest token')

        # valid token
        response = self.c.post('/user/login_status', {'token':self.token})
        print (response.content)

        # invalid token test
        response = self.c.post('/user/login_status', {'token':self.md5+":"+self.expire_time+":0"})
        print (response.content)

        # expire token
        response = self.c.post('/user/login_status', {'token':self.md5+":1300721018:"+self.user_id})
        print (response.content)

    # test logout
    def test_logout(self):
        print ('\ntest logout')
        response = self.c.post('/user/logout', {'token':self.token})
        print (response.content)

    # test data_pull
    def test_data_pull(self):
        print ('\ntest data_pull')
        response = self.c.get('/user/data_pull', {'token':self.token})
        print (response.content)
