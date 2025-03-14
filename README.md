# EggScript
A language I made because I felt like it. Built in C#.

## Points of interest
- Recognizes strings and numbers and booleans
- Operators, including a string concatenation operator (yay!)
- Unary operators (was hard to make)
- Parentheses (hard thing I made without chatgpt help :))
- Print keyword that prints stuff to the console
- Fully functional comments, both the // kind and the /* */ kind
- Mandatory semicolons, because I'm addicted to those
- Boolean operators are & |, not && || (those are weird)
- Variables! Strongly typed, also supports const variables
- Slightly readable source code, read today!

## Example program
```
/* Print stuff */
print("Hi!");
print("I like EggScript");

/* Variables */
const a = 2;
const b = 3;
var c = (a + b) * 3;
print(c); // 15

/* This part looks different from the others
   but I don't know what to name it */
c = c / 5;
print(c); // 3
print(c > 2 & c < 5); // True
```
