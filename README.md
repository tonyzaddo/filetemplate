# filetemplate

Copy input file to output file replacing marked values in file.

/f :: From File  
/t :: Output File  
  
eg: filetemplate /f c:\input.txt /t c:\output.txt  
  
The program will prompt for a list of words. Up to 30 words may be entered.  
  
The program will replace values in the input file as follows:  
@0 :: Replaced with first word enterd  
@1 :: Replaced with second word entered  
@n :: Where n is a number between 0 and 30, will be replaced with word n entered  
  
^0 :: Replaced with first word enterd in uppercase  
^1 :: Replaced with second word entered in uppercase  
^n :: Where n is a number between 0 and 30, will be replaced with word n entered in uppercase  

*** Ensure you enter at least as many words as marked up numbers in input file, otherwise will error ***
  
Use Cases  
=========  
  
I created this program to generate builerplace code based on a template.  I hade a large number of programs that just had name changes and I could not use a generic.  

