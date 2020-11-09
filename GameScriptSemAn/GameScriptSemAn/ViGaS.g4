grammar ViGaS;

script: (textureDefinition | modelDefinition | tileDefinition | baseDefinition | regionDefinition)*;

textureDefinition: TEXTURE textureId=ID FROM fileName=STRING;

modelDefinition: MODEL modelId=ID FROM textureId=ID animationDefinition+ END;
animationDefinition: ANIMATION animationId=ID LOOPING? frameDefinition+ END;
frameDefinition: FRAME x=NUMBER ',' y=NUMBER ',' width=NUMBER ',' height=NUMBER ',' duration=NUMBER;

tileDefinition: TILE tileId=ID FROM modelId=ID WALKABLE?;

baseDefinition: BASE baseRef=REFERENCE FROM baseClass=ID baseBody END;
baseBody: initBlock variablesBlock? runBlock*;

regionDefinition: REGION regionRef=REFERENCE regionBody END;
regionBody: initBlock instanceDefinition*;

instanceDefinition: INSTANCE instanceRef=REFERENCE? FROM baseRef=REFERENCE initBlock END;

initBlock: (assignmentStatement | functionCallStatement)*;

variablesBlock: VARIABLES variableDeclaration* END;

runBlock: RUN WHEN eventTypeName=ID statementList END;

statementList: statement*;

statement: assignmentStatement | ifStatement | whileStatement | repeatStatement | functionCallStatement | RETURN | COMMENT;

expression
	: left=expression multiplOperator right=expression		#multiplExpression
	| left=expression additiveOperator right=expression		#additiveExpression
	| left=expression logicalOperator right=expression		#logicalExpression
	| left=expression compOperator right=expression			#compExpression
	| '(' expression ')'									#parenExpression
	| '[' expression (',' expression)* ']'					#arrayExpression
	| functionCallStatement									#funcExpression
	| REFERENCE												#refExpression
	| param=ID												#paramExpression
	| path													#pathExpression
	| STRING												#stringExpression
	| NUMBER												#numberExpression
	| BOOLEAN												#boolExpression
	| NULL													#nullExpression
	| NOT expression										#notExpression
	;

path: (ref=REFERENCE | param=ID) ('.' (parts+=VARNAME | parts+=ID))+;

functionParameterList: expression (',' expression)*;

variableDeclaration: varName=VARNAME IS typeName=ID (WITHVALUE expression)?;
assignmentStatement: SET path TO? expression;

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

OR: 'or';
AND: 'and';
NOT: 'not';
XOR: 'xor';

PLUS: '+';
MINUS: '-';
MULT: '*';
DIV: '/';

COMMENT: '//' (~[\r\n])* -> skip;

//---------------------------KEYWORDS---------------------------

TEXTURE: 'texture';
MODEL: 'model';
ANIMATION: 'animation';
LOOPING: 'looping';
FRAME: 'frame';
TILE: 'tile';
WALKABLE: 'walkable';
BASE: 'base';
REGION: 'region';
INSTANCE: 'instance';
FROM: 'from';

VARIABLES: 'variables';
IS: 'is';
WITHVALUE: 'withvalue';

INIT: 'init';

RUN: 'run';
WHEN: 'when';

IF: 'if';
THEN: 'then';
ELSE: 'else';

WHILE: 'while';
REPEAT: 'repeat';
BREAK: 'break';

RETURN: 'return';

SET: 'set';
TO: 'to';

END: 'end';

BRACKET_OPEN: '(';
BRACKET_CLOSE: ')';
COMMA: ',';
DOT: '.';

THIS: 'this';
NULL: 'null';
BOOLEAN: 'true' | 'false';
STRING: '"' (~[\r\n])* '"';
NUMBER: [0-9]+ ('.' [0-9]+)?;

REFERENCE: '$' ID;
ID: [A-Z][a-zA-Z0-9_]*;
VARNAME: [a-z][a-zA-Z0-9_]*;
WS: (' '| '\t' | '\n' | '\r') -> skip;