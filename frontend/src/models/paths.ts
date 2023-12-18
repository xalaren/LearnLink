export class Paths {
    static readonly homePath = '/';
    static readonly loginPath = '/login';
    static readonly registerPath = '/register';
    static readonly profilePath = '/profile';
    static readonly userCoursesPath = '/profile/courses';
    static readonly editUserPath = '/profile/edit/user';
    static readonly editPasswordPath = '/profile/edit/password';
    static readonly courseViewPath = '/courses';
    static readonly courseViewFullPath = this.courseViewPath + '/:courseId';
    static readonly moduleViewPath = '/modules';
    static readonly moduleViewFullPath = `${this.courseViewFullPath}${this.moduleViewPath}/:moduleId`;
}