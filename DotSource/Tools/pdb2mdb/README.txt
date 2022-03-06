直接使用VS编译出来的DLL和PDB无法用于Unity中的调试,需要借助于pdb2mdb工具,转换为mdb文件才可以.

一般情况下,可以使用Unity的安装目录下:Editor\Data\MonoBleedingEdge\lib\mono\4.5\pdb2mdb.exe来处理

为了简化使用,此文件来自于https://gist.github.com/jbevain/ba23149da8369e4a966f#file-pdb2mdb-exe,
可以将工具当做一个独立的文件来使用

使用方法:pdb2mdb.exe XXXXX.dll