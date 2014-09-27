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
	image = FileField("image",validators=[DataRequired()])

	def set_by_goal(self, goal):
		self.goal_id.data = goal.goal_id
		self.goal_name.data = goal.goal_name
		self.description.data = goal.description
		categorys = [c.category_name for c in goal.categories]
		self.categories = ",".join(categorys)

	def to_goal(self, goal):
		goal.goal_name = self.goal_name.data
		goal.description = self.description.data
		return goal

class CategoryForm(BaseForm):
	category_name = TextField("category_name", validators=[DataRequired()])
	description = TextAreaField("description")