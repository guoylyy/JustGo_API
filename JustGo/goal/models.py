from django.db import models
from user.models import User
from uploadfile.models import UploadFile

# Create your models here.

class Category(models.Model):
    name = models.CharField(max_length=50, unique=True)

class Goal(models.Model):
    name = models.CharField(max_length=50, unique=True)
    category = models.ForeignKey(Category)
    detail = models.CharField(max_length=1023)

class UserGoal(models.Model):
    '''
    - privacy: public | private
    - finished: 0 for doing | 1 for done
    '''
    user = models.ForeignKey(User)
    goal = models.ForeignKey(Goal)
    privacy = models.CharField(max_length=30)
    finished = models.IntegerField(default=0)
    date_create = models.DateTimeField(auto_now_add=True)

class UserGoalCheckout(models.Model):
    user_goal = models.ForeignKey(UserGoal)
    picture = models.ForeignKey(UploadFile, null=True, default=None)
    comment = models.CharField(default="", max_length=1023)
    date_check = models.DateTimeField(auto_now_add=True)

class UserGoalComment(models.Model):
    comment_user = models.ForeignKey(User)
    checkout = models.ForeignKey(UserGoalCheckout)
    comment = models.CharField(max_length=1023)
    date_create = models.DateTimeField(auto_now=True)

class UserGoalAwesome(models.Model):
    awesome_user = models.ForeignKey(User)
    checkout = models.ForeignKey(UserGoalCheckout)
    date_create = models.DateTimeField(auto_now=True)

