# wad-cmd-tool

//library https://github.com/LoL-Fantome/Fantome.Libraries.League<br>
//help https://github.com/Crauzer/Obsidian<br>
//hashes https://github.com/CommunityDragon/CDTB<br>
//cmd tool https://github.com/Abbey92/wad-cmd-tool<br><br>
//------------------------------------------------------------------------------------------------------------------------English<br>
1.Export_all<br>
Export_all(siring wadfilepath)<br>
cmd:"wad cmd tool.exe" Export_all ashe.wad.client<br><br>
2.Save_all<br>
Save_all(siring directory)<br>
cmd:"wad cmd tool.exe" Save_all c:\\custom<br><br>
error<br>
cmd:"wad cmd tool.exe" Save_all c:\\custom\\assets<br><br>
3.Modify_all<br>
Modify_all(siring wadfilepath , siring directory)<br>
cmd:"wad cmd tool.exe" Modify_all ashe.wad.client c:\custom<br><br>
4.Export_Filter<br>
Export_Filter(siring wadfilepath , siring Filter)<br>
cmd:"wad cmd tool.exe" Export_Filter ashe.wad.client load<br><br>
//-----------------------------------------------------------------------------------------------------------------------------中文<br>
1.Export_all<br>
导出全部文件<br>
命令行:"wad cmd tool.exe" Export_all ashe.wad.client<br>
程序目录生成一个 "Export all" 文件夹,保存导出的文件<br><br>
2.Save_all<br>
保存全部文件<br>
命令行:"wad cmd tool.exe" Save_all c:\\custom<br>
解释:确保目录下包含皮肤文件(assets data文件夹)<br>
程序目录生成一个新文件 "new.wad.client" <br><br>
3.Modify_all<br>
替换全部条目<br>
命令行:"wad cmd tool.exe" Modify_all ashe.wad.client c:\\custom<br>
解释:确保目录下包含皮肤文件（assets data文件夹）<br>
程序目录生成一个新文件 "new.wad.client" <br><br>
4.Export_Filter<br>
导出被搜索的文件<br>
命令行："wad cmd tool.exe" Export_Filter ashe.wad.client load<br>
程序目录生成一个 "Export Filter" 文件夹,保存导出的文件<br><br>
