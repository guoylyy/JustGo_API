"""
	forms: account.py
	~~~~~~~~~~~~~~
"""
from flask_wtf import Form 
from wtforms import TextField, PasswordField, SubmitField, StringField,\
					BooleanField
from wtforms.validators import DataRequired

class BaseForm(Form):
	message = None
	def __init__(self, *args, **kwargs):
		kwargs['csrf_enabled'] = False
		super(BaseForm, self).__init__(*args, **kwargs)

class LoginForm(BaseForm):
	username = TextField('username', validators=[DataRequired()])
	password = PasswordField('password', validators=[DataRequired()])
	submit = SubmitField(("login"))