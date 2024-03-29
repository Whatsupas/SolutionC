# SolutionC
``
Exercise from Kattis testing system. Level - hard
``

## Issue
Kattis has **k** spare beds for visiting kittens. To make things a little bit easier to keep track of, she has made a 
booking system where kittens can request a bed for one or more nights.

She has just looked into system, and realized that there are many requests for beds. In fact there are too many 
to handle manually. She wants to offer beds to as many different kittens as possible, but she only has **k** beds. Can
you help her figure out how to accommodate as many kittens as possible?

Note that a kitten will only come if it can stay the whole time it wants to borrow a spare bed.

## Input
The first line of input contains two integers **n** and **k** (1 <= **k** < **n** <= 100000), the number of potential 
visiting kittens and the number of beds. Then follow **n** lines, each containing two integers **x(i)** and **y(i)** meaning that 
kitten **i** wants to arrive at time **x(i)** and leave at time **y(i)**. This means that two kittens **i** and **j** where 
y(i) = x(i) can use the same bed, as one kitten leaves at the same time as the other arrives. You may assume
that  
0 <= x(i) < y(i) <= 1000000000.
## Output
Output the maximum number of kittens that can be housed given the requests

### Sample 1 
```
Input     Output 
3 1         2
1 2
2 3 
2 3
```
### Sample 2
```
Input     Output 
4 1         3
1 3
4 6
7 8
2 5
```
### Sample 3
```
Input     Output 
5 2         3
1 4
5 9
2 7
3 8
6 10
```
![C3](https://user-images.githubusercontent.com/47819609/177035846-a366263d-c40b-4081-aa20-5ac12edfc28c.jpg)
