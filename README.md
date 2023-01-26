# Translator From CSharp To Python

# Checkout
[Translator WebSite](https://martynovaa.bsite.net/)

# Состав команды
* Кирилл Борисенко
* Александр Мартынов

# Группа
* Б9119-09.03.04прогин(3)

# Modules
* Lexical Analyzer (regex)
* Syntax Parser (top-down)
* Semantic Analyzer
* Code Generator

# Available constructions
## DataTypes `int float bool char string`
> input
```cs
int a = 5;
float b = 4.5;
bool c = true;
char d = 'd';
string e = "string";
```
> output
``` python
a = 5
b = 4.5
c = True
d = 'd'
e = "string"
```
## Functions
> input
```cs
int func(int a, int b)
{
	return a + b;
}
int c = func(1, 4);
```
> output
```python
func(a, b):
	return a + b
c = func(1, 4)
```
## Unary operators `++ --`
> input
```cs
int a = 4;
a++;
a--;
```
> output
``` python
a = 4
a++
a--
```
## Binary operators `= / + * - == != < > <= >=`
> input
```cs
int a = 4 + 2 - 18 * 6 / 2;
bool c = 5 >= 10;
```
> output
```python
a = 4 + 2 - 18 * 6 / 2
c = 5 >= 10
```
## Strings
> input
```cs
string a = "hello" + " " + "world";
```
> output
``` python
a = "hello" + " " + "world"
```
## Loops `while`
> input
```cs
int a = 0;
int b = 5;
while (a > b)
{
	a++;
}
```
> output
```python
a = 0
b = 5
while (a > b):
	a++
```
## Console print
> input
```cs
Console.WriteLine("hello world" + " ! ");
Console.WriteLine(5);
```
> output
```python
print("hello world" + " ! ")
print(5)
```
## Conditional orerator `(if) (else if) (else)`
> input
```cs
int a = 0;
int b = 5;

if (a < b)
{
	int c = 5;
}
else if (a > -5)
{
	int c = 4;
}
else
{
	int c = 7;
}
```
> output
```python
a = 0
b = 5
if (a < b):
	c = 5
elif (a > -5):
	c = 4
else:
	c = 7
```

# Error Handler
> VariableIncorrectDataTypeError: Attempt To Use STRING Instead INTEGER
```cs
int a = "string";
```
> FunctionDeclarationWithoutReturn: Attempt To Declare Function "func" With "INTEGER" DataType Without "Return" Instruction
```cs
int func(int a, string b)
{
	Console.WriteLine("hi");
}
```
> VariableNotDeclaredError: Attempt To Use Not Declared Variable "a"
```cs
a = 5;
```
> VariableIncorrectDataTypeError: Attempt To Use VOID Instead INTEGER
```cs
void func()
{
	int a = 5;
}

int b = func();
```
> FunctionNotDeclaredError: Attempt To Use Not Declared Function "func"
```cs
int b = func();
```
> VariableIncorrectDataTypeError: Attempt To Use FLOAT Instead BOOL
```cs
float func()
{
	return 1.0;
}

bool b = func();
```
> FunctionIsAlreadyDeclaredError: Attempt To Declare Against a Function "func" with parameters {INTEGER a}
```cs
float func(int a)
{
	return 1.0;
}

float func(int a)
{
	return 1.0;
}
```

etc...
