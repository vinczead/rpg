grammar ViGaS;

script: (baseDefinition | instanceDefinition)*;

baseDefinition: baseHeader definitionBody;
baseHeader: DEFINE baseId FROM baseClass;

instanceDefinition: instanceHeader definitionBody;
instanceHeader: INSTANTIATE instanceId FROM baseId;

definitionBody: variablesBlock? initBlock runBlock*;

initBlock: INIT statementList END;

variablesBlock: VARIABLES variableDeclaration* END;

runBlock: RUN WHEN eventTypeName statementList END;

statementList: statement*;

statement: assignmentStatement | ifStatement | whileStatement | repeatStatement | functionCallStatement | RETURN | COMMENT;

expression
	: functionCallStatement									#funcExpression
	| path													#pathExpression
	| STRING												#stringExpression
	| REFERENCE												#refExpression
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

path: varPath | refPath;

refPath: REFERENCE ('.' varName)*;
varPath: varName ('.' varName)*;

functionParameterList: expression (',' expression)*;

variableDeclaration: varName IS typeName (WITHVALUE expression)?;
assignmentStatement: SET path TO expression;
ifStatement: IF expression THEN statementList (elseStatement)? END;
elseStatement: ELSE statementList;
whileStatement: WHILE expression statementList REPEAT;
repeatStatement: REPEAT statementList WHILE expression;

functionCallStatement: functionName '(' functionParameterList? ')';

additiveOperator: PLUS | MINUS;
multiplOperator: MULT | DIV;

compOperator: LT | GT | LTE | GTE | EQ | NEQ;
logicalOperator: OR | AND | XOR;

//---------------------------IDS---------------------------

baseId: ID;
baseClass: ID;
instanceId: ID;
typeName: ID;
varName: ID;
eventTypeName: ID;
functionName: ID;

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

DEFINE: [Dd]'efine';
INSTANTIATE: [Ii]'nstantiate';
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
TO: 'To' | 'to';

END: 'End' | 'end';

BRACKET_OPEN: '(';
BRACKET_CLOSE: ')';
COMMA: ',';
DOT: '.';

NULL: 'Null' | 'null';
BOOLEAN: 'True' | 'true' | 'False' | 'false';
STRING: '"' (~[\r\n])* '"';
NUMBER: [0-9]+ ('.' [0-9]+)?;
REFERENCE: '$' ID;

ID: [a-zA-Z][a-zA-Z0-9_]*;
WS: (' '| '\t' | '\n' | '\r') -> skip;