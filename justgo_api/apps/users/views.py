from django.views.decorators.csrf import csrf_exempt
from django.http import HttpResponse
from django.core.paginator import Paginator
from apps.users.models import *
from apps.conf.config import get_config,get_page_result
from apps.conf.helper import check_token
import simplejson
import hashlib
import datetime
import time


@csrf_exempt
def facebook_login(request):
    """ Login system by facebook profile
    @Input:

    @Output:

    """
    try:
        facebook_token = request.POST.get('token')
        nickname = request.POST.get('nickname')
        facebook_id = request.POST.get('id')
        users = User.objects.filter(facebook_id=facebook_id)
        if len(users) > 0:
            return __perform_login(users[0])
        else:
            u = User(facebook_token=facebook_token, nickname=nickname, facebook_id=facebook_id, icon='')
            u.save()
            if u.id is not None:
                return __perform_login(u)
            else:
                return HttpResponse(get_page_result('500'))
    except Exception, e:
        return HttpResponse(get_page_result('500'))        

@csrf_exempt
def login_status(request):
    try :
        token = request.POST.get('token')
        response = check_token(token)
        if response is True:
            return HttpResponse(get_page_result('200',"null"))
        else:
            return response
    except Exception as e:
        return HttpResponse(get_page_result('500'))

@csrf_exempt
def logout(request):
    try :
        token = request.POST.get('token')
        response = check_token(token)
        if response is True:
            user = User.objects.filter(auth_token=token)[0]
            user.auth_token = ''
            user.save()
            return HttpResponse(get_page_result('200',"null"))
        else:
            return response
    except Exception as e:
        return HttpResponse(get_page_result('500'))

def data_pull(request):
    try :
        token = request.GET.get('token')
        response = check_token(token)
        if response is True:
            md5, expire, user_id = token.split(":", 3)
            users = User.objects.filter(id=user_id)
            if len(users) == 0:
                return HttpResponse(get_page_result('404'))
            # render user data
            user, data = users[0], {}
            data['id'] = user.id
            data['nickname'] = user.nickname
            data['icon'] = user.icon

            return HttpResponse(get_page_result('200',simplejson.dumps(str(data))))
        else:
            return response
    except Exception as e:
        print(e)
        return HttpResponse(get_page_result('500'))

def user_profile(request):
    token  = request.GET.get('token')
    user_id = request.GET.get('id')
    u = User.objects.filter(auth_token=token).first()
    user = User.objects.get(id=user_id)

    data_map = user.get_data_map()
    if len(Follow.objects.filter(user_fan=u, user_star=user)) > 0:
        data_map['is_follow'] = True
    else:
        data_map['is_follow'] = False

    if user is not None:
        return HttpResponse(get_page_result('200',simplejson.dumps(str(data_map))))
    else:
        return HttpResponse(get_page_result('404'))

def follow(request):
    try:
        token  = request.GET.get('token')
        user_id = request.GET.get('id')
        response = check_token(token)
        if response is True:
            u = User.objects.filter(auth_token=token).first()
            u_target = User.objects.get(id=user_id)
            if u is not None and u_target is not None:
                Follow(user_star=u, user_fan=u_target).save()
                return HttpResponse(get_page_result('200'))
            else:
                return HttpResponse(get_page_result('500'))
        else:
            return response
    except Exception, e:
        print(e)
        return HttpResponse(get_page_result('500'))
    

def unfollow(request):
    try:
        token  = request.GET.get('token')
        user_id = request.GET.get('id')
        response = check_token(token)
        if response is True:
            u = User.objects.filter(auth_token=token).first()
            u_target = User.objects.get(id=user_id)
            if u is not None and u_target is not None:
                Follow.objects.filter(user_star=u_target, user_fan=u).delete()
                return HttpResponse(get_page_result('200'))
            else:
                return HttpResponse(get_page_result('500'))
        else:
            return response
    except Exception, e:
        print(e)
        return HttpResponse(get_page_result('500'))
    
def follows(request):
    try:
        token  = request.GET.get('token')
        response = check_token(token)
        if response is True:
            u = User.objects.filter(auth_token=token).first()
            results = [f.user_fan.get_data_map() for f in Follow.objects.filter(user_star=u)]
            return HttpResponse(get_page_result('200',simplejson.dumps(str(results))))
        else:
            return response        
    except Exception, e:
        return HttpResponse(get_page_result('500'))

def followings(request):
    try:
        token  = request.GET.get('token')
        response = check_token(token)
        if response is True:
            u = User.objects.filter(auth_token=token).first()
            results = [f.user_fan.get_data_map() for f in Follow.objects.filter(user_fan=u)]
            return HttpResponse(get_page_result('200',simplejson.dumps(str(results))))
        else:
            return response        
    except Exception, e:
        return HttpResponse(get_page_result('500'))



######################################################
# private functions start here
#
def __get_token_result(result,token):
    return str("{\"token\":\""+token+"\"}")

def __make_token(id):
    # generate expire date
    expire_day = get_config('session_expire')
    expire_time = (datetime.datetime.now() + datetime.timedelta(expire_day,0)).timetuple();
    expire_datetime = time.mktime(expire_time)
    expire = str(int(expire_datetime))
    # token
    s = '%s:%s:%s' % (id, expire, get_config('md5_random'))
    md5 = hashlib.md5(s.encode('utf-8')).hexdigest()

    return md5+":"+expire+":"+str(id)

def __get_default_portrait():
    '''
    return UploadFile object
    '''
    try :
        name = get_config('default_portrait_name')
        portrait = UploadFile(file_name=name)
        portrait.save()
        if portrait.id is not None:
            return portrait
        print ('Error: Default portrait save fail!')
        return None
    except Exception as e:
        print (e)
        return None

def __perform_login(user):
    '''
    generate token and return
    '''
    try:
        user.auth_token = __make_token(user.id) 
        user.save()
        return HttpResponse(get_page_result('200',__get_token_result('success',user.auth_token)))
    except Exception as e:
        return HttpResponse(get_page_result('023'))
# Private function ends
###############################################
