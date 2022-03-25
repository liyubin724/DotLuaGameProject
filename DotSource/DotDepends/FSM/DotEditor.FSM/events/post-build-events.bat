@ECHO OFF

SET SolutionDir=%1
SET ProjectDir=%2
SET ConfigurationName=%3
SET ProjectName=%4
SET OutputDir=%5

SET TargetDir=%ProjectDir%bin\%ConfigurationName%

SET Pdb2mdbToolPath=%SolutionDir%Tools\pdb2mdb\pdb2mdb.exe

IF NOT EXIST %OutputDir% (
    MD %OutputDir%
)

SETLOCAL enabledelayedexpansion
FOR /r %TargetDir%\ %%f IN (*.dll) DO (
    REM ECHO %%f

    SET fileFullPath=%%f
    SET fileName=%%~nf

    REM ECHO !fileName!

    IF [!fileName!] NEQ [UnityEngine] (
        if [!fileName!] NEQ [UnityEditor] (
            !Pdb2mdbToolPath! !fileFullPath!

            SET Prefix=!fileName:~0,9!
            REM ECHO !Prefix!
            IF [!Prefix!] EQU [DotEditor] (
                IF NOT EXIST !OutputDir!Editor (
                    MD !OutputDir!Editor
                )
                COPY !fileFullPath! !OutputDir!Editor
                COPY !fileFullPath!.mdb !OutputDir!Editor
            ) ELSE IF [!Prefix!] EQU [DotEngine] (
                COPY !fileFullPath! !OutputDir!
                COPY !fileFullPath!.mdb !OutputDir!
            )
        )
    )
)
