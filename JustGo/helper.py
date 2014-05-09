from django.http import HttpResponse
from user.models import *
from config import get_page_result
import time

def check_token(token):
    """
        To check the token if there are something wrong
    """
    if token is None or len(token) == 0:
        return HttpResponse(get_page_result('414'))
    if not __check_token(token):
        return HttpResponse(get_page_result('022')) #invalid token
    if not __check_expire(token):
        return HttpResponse(get_page_result('021')) #expired token
    return True

def __check_token(token):
    '''
    return
    - True for matched token
    - False for invalid token
    '''
    try :
        md5, expire, id = token.split(":", 3)
        user_token = User.objects.get(id=id).auth_token
        if user_token == token:
            return True
        print ('Token auth Fail: id %s user token not match' % id)
        return False
    except Exception as e:
        print (e)
        return False

def __check_expire(token):
    '''
    return 
    - True for expire token
    - False for valid token
    - None for error token
    '''
    try :
        md5, expire, id = token.split(":", 3)
        if int(expire) < time.time():
            return False
        return True
    except Exception as e:
        print (e)
        return None