from django.db import models

# Create your models here.

class UploadFile(models.Model):
    file_name = models.CharField(max_length=200)
    file_path = models.CharField(max_length=200, null=True)
    file_type = models.CharField(max_length=200, null=True)
