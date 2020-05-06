grammar ViGaS;

script: (baseDefinition | regionDefinition)*;

baseDefinition: BASE baseRef=REFERENCE FROM baseClass=ID baseBody END;
baseBody: initBlock variablesBlock? runBlock*;

regionDefinition: REGION regionRef=REFERENCE regionBody END;
regionBody: initBlock instanceDefinition*;

instanceDefinition: INSTANCE instanceRef=REFERENCE? FROM baseRef=REFERENCE initBlock END;

initBlock: (propertyAssignmentStatement | variableAssignmentStatement | functionCallStatement)*;

variablesBlock: VARIABLES variableDeclaration* END;

runBlock: RUN WHEN eventTypeName=ID statementList END;

statementList: statement*;

statement: propertyAssignmentStatement | variableAssignmentStatement | ifStatement | whileStatement | repeatStatement | functionCallStatement | RETURN | COMMENT;

expression
	: functionCallStatement									#funcExpression
	| REFERENCE												#refExpression
	| param=ID												#paramExpression
	| varPath												#varPathExpression
	| propPath												#propPathExpression
	| STRING												#stringExpression
	| NUMBER												#numberExpression
	| BOOLEAN												#boolExpression
	| NULL													#nullExpression
	| NOT expression										#notExpression
	| left=expression multiplOperator right=expression		#multiplExpression
	| left=expression additiveOperator right=expression		#additiveExpression
	| left=expression logicalOperator right=expression		#logicalExpression
	| left=expression compOperator right=expression			#compExpression
	| '(' expression ')'									#parenExpression
	| '[' expression (',' expression)* ']'					#arrayExpression
	;

varPath: (ref=REFERENCE | param=ID) '.' ((parts+=VARNAME | parts+=ID) '.')* parts+=VARNAME;

propPath: (ref=REFERENCE | param=ID) '.' ((parts+=VARNAME | parts+=ID) '.')* parts+=ID;

functionParameterList: expression (',' expression)*;

variableDeclaration: varName=VARNAME IS typeName=ID (WITHVALUE expression)?;
propertyAssignmentStatement: SET propPath TO? expression;
variableAssignmentStatement: SETVAR varPath TO? expression;

ifStatement: IF expression THEN statementList (elseStatement)? END;
elseStatement: ELSE statementList;
whileStatement: WHILE expression statementList REPEAT;
repeatStatement: REPEAT statementList WHILE expression;

functionCallStatement: functionName=ID '(' functionParameterList? ')';

additiveOperator: PLUS | MINUS;
multiplOperator: MULT | DIV;

compOperator: LT | GT | LTE | GTE | EQ | NEQ;
logicalOperator: OR | AND | XOR;

//---------------------------OPERATORS--------------------------- 


LT: '<';
GT: '>';
LTE: '<=';
GTE: '>=';
EQ: '=';
NEQ: '<>';

OR: 'Or' | 'or';
AND: 'And' | 'and';
NOT: 'Not' | 'not';
XOR: 'Xor' | 'xor';

PLUS: '+';
MINUS: '-';
MULT: '*';
DIV: '/';

COMMENT: '//' (~[\r\n])* -> skip;

//---------------------------KEYWORDS---------------------------

BASE: [Bb]'ase';
REGION: [Rr]'egion';
INSTANCE: [Ii]'nstance';
FROM: [Ff]'rom';

VARIABLES: [Vv]'ariables';
IS: [Ii]'s';
WITHVALUE: 'WithValue' | 'withvalue';
PARAMETER: [Pp]'arameter';
SHARED: [Ss]'hared';

INIT: [Ii]'nit';

RUN: [Rr]'un';
WHEN: [Ww]'hen';

IF: 'If' | 'if';
THEN: 'Then' | 'then';
ELSE: 'Else' | 'else';

WHILE: 'While' | 'while';
REPEAT: 'Repeat' | 'repeat';
BREAK: 'Break' | 'break';

RETURN: 'Return' | 'return';

SET: 'Set' | 'set';
SETVAR: 'SetVar' | 'setvar';
TO: 'To' | 'to';

END: 'End' | 'end';

BRACKET_OPEN: '(';
BRACKET_CLOSE: ')';
COMMA: ',';
DOT: '.';

THIS: [Tt]'his';
NULL: 'Null' | 'null';
BOOLEAN: 'True' | 'true' | 'False' | 'false';
STRING: '"' (~[\r\n])* '"';
NUMBER: [0-9]+ ('.' [0-9]+)?;

REFERENCE: '$' ID;
ID: [A-Z][a-zA-Z0-9_]*;
VARNAME: [a-z][a-zA-Z0-9_]*;
WS: (' '| '\t' | '\n' | '\r') -> skip;