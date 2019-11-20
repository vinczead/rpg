grammar GameScript;

program: scriptStart variablesBlock? runBlock*;

scriptStart: SCRIPT scriptName;

variablesBlock: VARIABLES variableDeclaration+;

runBlock: RUN eventTypeName statementList END;

statementList: statement+;
statement: assignmentStatement | ifStatement | functionCallStatement | RETURN | COMMENT;

expression
	: functionCallStatement									#funcExpression
	| path													#pathExpression
	| STRING												#stringExpression
	| REFERENCE												#refExpression
	| NUMBER												#numberExpression
	| BOOLEAN												#boolExpression
	| NOT expression										#notExpression
	| left=expression compOperator right=expression			#compExpression
	| left=expression multiplOperator right=expression		#multiplExpression
	| left=expression additiveOperator right=expression		#additiveExpression
	| left=expression logicalOperator right=expression		#logicalExpression
	| '(' expression ')'									#parenExpression
	;

path: varPath | refPath;

refPath: REFERENCE ('.' varName)*;
varPath: varName ('.' varName)*;

functionParameterList: expression (',' expression)*;

variableDeclaration: varName IS typeName (WITHVALUE expression)?;
assignmentStatement: SET path TO expression;
ifStatement: IF expression THEN statementList (elseStatement)? ENDIF;
elseStatement: ELSE statementList;
functionCallStatement: path? '=>' functionName '(' functionParameterList ')';

additiveOperator: PLUS | MINUS;
multiplOperator: MULT | DIV;

compOperator: LT | GT | LTE | GTE | EQ | NEQ;
logicalOperator: OR | AND | XOR;

scriptName: ID;
typeName: ID;
varName: ID;
eventTypeName: ID;
functionName: ID;

ARROW: '=>';

LT: '<';
GT: '>';
LTE: '<=';
GTE: '>=';
EQ: '=';
NEQ: '<>';

OR: 'Or';
AND: 'And';
NOT: 'Not';
XOR: 'Xor';

COMMENT: '//' (~[\r\n])* -> skip;

PLUS: '+';
MINUS: '-';
MULT: '*';
DIV: '/';

SCRIPT: 'Script';
IF: 'If';
THEN: 'Then';
ELSE: 'Else';
ENDIF: 'EndIf';
RETURN: 'Return';
VARIABLES: 'Variables';
IS: 'Is';
WITHVALUE: 'WithValue';
SET: 'Set';
TO: 'To';
RUN: 'Run';
END: 'End';

BRACKET_OPEN: '(';
BRACKET_CLOSE: ')';
COMMA: ',';
DOT: '.';

NULL: 'Null';
BOOLEAN: 'True' | 'False';
STRING: '"' (~[\r\n])* '"';
NUMBER: [0-9]+ ('.' [0-9]+)?;
REFERENCE: '$' ID;

ID: [a-zA-Z][a-zA-Z0-9_]*;
WS: (' '| '\t' | '\n' | '\r') -> skip;