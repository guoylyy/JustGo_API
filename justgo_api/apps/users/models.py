#Users Model
from django.db import models

class User(models.Model):
    date_create = models.DateTimeField(auto_now_add=True)
    auth_token = models.CharField(max_length=200)
    facebook_token = models.CharField(max_length=200)
    facebook_id = models.CharField(max_length=200)
    nickname = models.CharField(max_length=100)
    icon = models.CharField(max_length=200)

    def get_data_map(self):
        data = {}
        data['id'] = self.id
        data['nickname'] = self.nickname
        data['icon'] = self.icon
        return data

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