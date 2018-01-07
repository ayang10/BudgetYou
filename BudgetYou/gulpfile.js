var gulp = require('gulp'),
    sass = require('gulp-sass'),
    minifyCSS = require('gulp-minify-css');

var config = {
    bowerDir: './bower_components'
}

gulp.task('sass', function () {
    return gulp.src('./assets/sass/*.scss')
        .pipe(sass())
        .pipe(gulp.dest('./assets/css'));
});

gulp.task('watch', function () {
    gulp.watch('./assets/sass/*.scss', ['sass']);
});

gulp.task('minify-css', function () {
    return gulp.src('./assets/css/style.css')
        .pipe(minifyCSS({ keepSpecialComments: 1 }))
        .pipe(gulp.dest('./assets/minify-css'));
});

gulp.task('default', ['sass', 'watch']);