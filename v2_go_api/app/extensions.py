# -*- coding: utf-8 -*-
from flask_sqlalchemy import SQLAlchemy
from flask.ext.login import LoginManager
from sqlalchemy_imageattach.stores.fs import FileSystemStore

__all__ = ['db', 'fs_store','login_manager']

IMAGE_UPLOAD_URL = '/Users/globit/git/JustGo_Project/v2_go_api/userimages'
IMAGE_URL = 'file:///Users/globit/git/JustGo_Project/v2_go_api/userimages/'

db = SQLAlchemy()
fs_store = FileSystemStore(IMAGE_UPLOAD_URL, IMAGE_URL)
login_manager = LoginManager()