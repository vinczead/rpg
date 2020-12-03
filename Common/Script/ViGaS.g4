grammar ViGaS;

script: (textureDefinition | modelDefinition | tileDefinition | baseDefinition | regionDefinition)* playerDefinition? EOF;
breedScript: baseDefinition EOF;

textureDefinition: TEXTURE textureId=ID FROM fileName=STRING;

modelDefinition: MODEL modelId=ID FROM textureId=ID frameSizeBlock collisionBoxBlock? animationDefinition+ END;
frameSizeBlock: FRAME SIZE width=NUMBER ',' height=NUMBER;
collisionBoxBlock: COLLISION x=NUMBER ',' y=NUMBER ',' width=NUMBER ',' height=NUMBER;
animationDefinition: ANIMATION animationId=ID LOOPING? frameDefinition+ END;
frameDefinition: FRAME AT? x=NUMBER ',' y=NUMBER (',' | FOR) duration=NUMBER MS?;

tileDefinition: TILE tileId=ID FROM modelId=ID WALKABLE?;

baseDefinition: BASE baseRef=ID FROM baseClass=ID baseBody END;
baseBody: initBlock variablesBlock? runBlock*;

regionDefinition: REGION regionRef=ID SIZE? width=NUMBER ',' height=NUMBER regionBody END;
regionBody: tileSizeBlock tilesBlock instanceDefinition*;
tileSizeBlock: TILE SIZE width=NUMBER ',' height=NUMBER;
tilesBlock: TILES expression* END;

instanceDefinition: INSTANCE instanceRef=ID? OF baseRef=ID AT x=NUMBER ',' y=NUMBER (initBlock END)?;

playerDefinition: PLAYER instanceId=ID?;

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
AT: 'at';
FOR: 'for';
MS: 'ms';
SIZE: 'size';
COLLISION: 'collision';
WALKABLE: 'walkable';
BASE: 'breed';
REGION: 'region';
TILES: 'tiles';
INSTANCE: 'instance';
FROM: 'from';
PLAYER: 'player';
OF: 'of';

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

NULL: 'Null' | 'null';
BOOLEAN: 'True' | 'true' | 'False' | 'false';
STRING: '"' (~[\r\n])* '"';
NUMBER: [0-9]+ ('.' [0-9]+)?;

REFERENCE: '$' ID;
ID: [A-Z][a-zA-Z0-9_]*;
VARNAME: [a-z][a-zA-Z0-9_]*;
WS: (' '| '\t' | '\n' | '\r') -> skip;