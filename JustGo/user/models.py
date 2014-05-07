from django.db import models
from uploadfile.models import UploadFile

# Create your models here.

class User(models.Model):
    name = models.CharField(max_length=50)
    password = models.CharField(max_length=254)
    portrait = models.ForeignKey(UploadFile)
    email = models.EmailField(unique=True,max_length=254)    
    date_create = models.DateTimeField(auto_now_add=True)
    auth_token = models.CharField(max_length=200)

'''
- user_star is followed by user_fan
- deprecated: flag whether the follow relationship is still in use
'''
class Follow(models.Model):
    user_star = models.ForeignKey(User, related_name='user_star_set')
    user_fan = models.ForeignKey(User, related_name='user_fan_set')
    date_create = models.DateTimeField(auto_now_add=True)
    deprecated = models.IntegerField(default=0)

class UserToken(models.Model):
    user_id = models.ForeignKey(User)
    token = models.CharField(max_length=254)
