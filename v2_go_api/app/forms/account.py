"""
	forms: account.py
	~~~~~~~~~~~~~~
"""
from flask_wtf import Form 
from wtforms import TextField, PasswordField, SubmitField, StringField,\
					BooleanField
from wtforms.validators import DataRequired
from .base import BaseForm


class LoginForm(BaseForm):
	username = TextField('username', validators=[DataRequired()])
	password = PasswordField('password', validators=[DataRequired()])
	submit = SubmitField(("login"))