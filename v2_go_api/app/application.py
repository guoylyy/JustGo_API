# -*- coding: utf-8 -*-


import os
import logging
from flask import Flask, g
from app import configs, views
from app.views import account 

from flask.ext import restful

from extensions import db, login_manager
from apis import create_api

from apis.user_api import * 
from apis.goal_api import * 

from sqlalchemy_imageattach.stores.fs import FileSystemStore


__all__ = ['create_app']
DEFAULT_APP_NAME = 'app'

# Add blueprint instnace here
REGISTER_BLUE_PRINTS = (
        (account.instance,''),
        )
DEFAULT_MODULES = (
        (views.goal,'/backend/goal'),
        (views.category,'/backend/category'),
        (views.advertisement,'/backend/advertisement'),
    )

def create_app(config=None,app_name=None,is_test=False, modules=None):
    if modules is None:
        modules = DEFAULT_MODULES   

    if app_name is None:
        app_name = DEFAULT_APP_NAME
    
    app = Flask(app_name)

    configure_app(app, config)
    configure_extensions(app)
    confgiure_api(app)
    configure_blueprints(app)
    #configure_cache(app)
    configure_modules(app, modules)
    return app

def configure_app(app,config):
    app.config.from_object(configs.DefaultConfig())
    if config is not None:
        app.config.from_object(config)
    app.config.from_envvar('APP_CONFIG', silent=True)

def configure_extensions(app):
    db.init_app(app)
    login_manager.init_app(app)
    login_manager.login_view = '/login'

def confgiure_api(app):
    BASE_URL = '/' + app.config['VERSION']
    api = create_api(restful.Api(app), BASE_URL)

def configure_modules(app, modules):
    for module, url_prefix in modules:
        app.register_module(module, url_prefix=url_prefix)

def configure_blueprints(app):
    for blue,url_prefix in REGISTER_BLUE_PRINTS:
        app.register_blueprint(blue,url_prefix=url_prefix)