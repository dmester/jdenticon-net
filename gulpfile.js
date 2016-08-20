var gulp         = require("gulp"),
    del          = require("del"),
    msbuild      = require("gulp-msbuild"),
    assemblyInfo = require("gulp-dotnet-assembly-info"),
    rename       = require("gulp-rename"),
    zip          = require("gulp-zip"),
    replace      = require("gulp-replace"),
    exec         = require("child_process").exec,
    pack         = require("./package.json");

gulp.task("clean", function (cb) {
    del(["./~jdenticon-net.nuspec", "./obj"], cb);
});

gulp.task("build", ["assemblyInfo", "clean"], function () {
    return gulp.src("./jdenticon-net.sln")
        .pipe(msbuild({
            properties: { Configuration: 'Release' }
            })
        );
});

gulp.task("assemblyInfo", function () {
    return gulp.src('**/AssemblyInfo.cs')
        .pipe(assemblyInfo({
            copyright: function(value) { return "Copyright ® " + pack.author + " " + new Date().getFullYear(); },
            version: function(value) { return pack.version + ".0"; },
            fileVersion: function(value) { return pack.version + ".0"; }
        }))
        .pipe(gulp.dest('.'));
});

gulp.task("releaseInfo", function () {
    return gulp.src(["./license.txt"])
        .pipe(replace(/\{version\}/g, pack.version))
        .pipe(replace(/\{year\}/g, new Date().getFullYear()))
        .pipe(replace(/\{date\}/g, new Date().toISOString()))
        .pipe(rename(function (path) { path.extname = ".txt" }))
        .pipe(gulp.dest("obj"));
});

gulp.task("preparenuget", ["build"], function () {
    return gulp.src(["./jdenticon-net.nuspec"])
        .pipe(replace(/\{version\}/g, pack.version))
        .pipe(replace(/\{year\}/g, new Date().getFullYear()))
        .pipe(replace(/\{date\}/g, new Date().toISOString()))
        .pipe(rename(function (path) { path.basename = "~" + path.basename }))
        .pipe(gulp.dest("./"));
});

gulp.task("nuget", ["preparenuget", "build"], function (cb) {
    exec("\"./Utils/NuGet/nuget.exe\" pack ~jdenticon-net.nuspec -OutputDirectory releases", function (error, stdout, stderr) {
        if (error) {
            cb(error);
        } else {
            del(["./~jdenticon-net.nuspec"], cb);
        }
    });
});

gulp.task("createpackage", ["build", "releaseInfo"], function () {
    return gulp.src(["./obj/*", "./Jdenticon/bin/Release/*"])
        .pipe(zip("jdenticon-net-" + pack.version + ".zip"))
        .pipe(gulp.dest("releases"));
});

gulp.task("release", ["createpackage", "nuget"]);
