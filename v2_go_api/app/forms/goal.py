from flask_wtf import Form 
from wtforms import TextField, PasswordField, SubmitField, StringField,\
					BooleanField, TextAreaField, FileField
from wtforms.validators import DataRequired
from .base import BaseForm

class GoalForm(BaseForm):
	goal_id = TextField("goal_id")
	goal_name = TextField("goal_name", validators=[DataRequired()])
	description = TextAreaField("description")
	category_name = TextField("category_name")
	image = FileField("image")

	def set_by_goal(self, goal):
		self.goal_id.data = goal.goal_id
		self.goal_name.data = goal.goal_name
		self.description.data = goal.description

class CategoryForm(BaseForm):
	category_name = TextField("category_name", validators=[DataRequired()])
	description = TextAreaField("description", validators=[DataRequired()])