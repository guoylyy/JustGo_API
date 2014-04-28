def get_config(key):
    config = {
        'session_expire' : 30,
        'md5_random' : 'youdontknow',
        'default_portrait_name' : 'default_portrait.png',
    }
    
    return config[key]
