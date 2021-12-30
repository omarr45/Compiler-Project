# Tiny Language CFG Rules

- Non-Terminals
    1. Function_call
    2. Term
    3. Arithmatic_Op 
    4. Equation
    5. Expression
    6. Assignment_Statement
    7. Datatype 
    8. Declaration_Statement
    9. Write_Statement
    10. Read_Statement
    11. Return_Statement
    12. Condition_Op 
    13. Condition
    14. Boolean_Operator 
    15. Condition_Statement
    16. If_Statement
    17. Else_If_Statement
    18. Else_Statement
    19. Repeat_Statement
    20. Parameter
    21. Function_Declaration
    22. Function_Body
    23. Function_Statement
    24. Main_Function
    25. Program
    
1. Program **→** Program' Main_Function
2. **Program'** → Function_Statement Program' | e
3. Main_Function → Datatype main ( ) Function_Body
4. Function_Statement → Function_Declaration Function_Body
5. Function_Body → { **Statements** Return_Statement }
6. **Statements →** **Statement** **Statements'**
7. **Statements' →** **Statement Statements'** | e
8. **Statement** → Assignment_Statement ;| Declaration_Statement | Write_Statement | Read_Statement | Condition_Statement | If_Statement | Repeat_Statement | Function_Call ;
9. Function_Declaration → Datatype Function_Name ( **Parameters** )
10. **Parameters →** Parameter **Parameters'** | e
11. **Parameters' →** , Parameter **Parameters'** | e
12. Parameter → Datatype Identifier
13. Function_Name → Identifier
14. Repeat_Statement → repeat **Statements** until Condition_Statement
15. Condition_Statement → Condition **Conditions** 
16. **Conditions** → Boolean_Op Condition **Conditions** | e
17. If_statement  → if Condition_statement then **StatementsOrReturn** **Rest_if** 
18. **Rest_if**  → Else_if_statement | Else_statement | end 
19. **StatementsOrReturn** → Statements **StatementsOrReturn** | Return_statement | e
20. Else_if_statement  → elseif Condition_statement then **StatementsOrReturn** **Rest_if** 
21. Else_statement  → else **Statements** end 
22. Return_Statement → return Expression ;
23. Read_Statement → read Identifier ;
24. Write_Statement → write **Write'** ;
25. **Write'** → Expression | endl 
26. Declaration_Statement → Datatype **Ids** ;
27. **Ids →** Identifier **Ids'** | Assignment_Statement **Ids'**
28. **Ids'** → , **Idds** |e
29. **Idds** → Identifier **Ids'** | Assignment_Statement **Ids'**
30. Datatype → int | float | string
31. Assignment_Statement → Identifier := Expression 
32. Expression → String | Term | Equation
33. Arithmatic_Op → Add_Op | Mult_Op
34. Boolean_Op → && | "||"
35. Condition_Op → < | > | = | <>
36. Function_Call → Function_Name ( **Identifiers** )
37. **Identifiers →** Identifier **Identifier'** | e
38. **Identifier' → ,** Identifier **Identifier'** | e
39. Condition  →  Identifier Condition_Op Expression
40. Add_Op →  + | -
41. Mult_Op →  * | /
42. Equation →  factor **Equation'**
43. **Equation'** →  Add_Op factor **Equation'** | epsilon
44. factor →  ( Equation )  | factor Mult_Op term | term
45. Term →  Number | Identifier | Function_Call