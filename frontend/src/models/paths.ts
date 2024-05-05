export class Paths {
    static readonly homePath = '/';
    static readonly publicPath = '/public/page';
    static readonly loginPath = '/login';
    static readonly registerPath = '/register';
    static readonly profilePath = '/profile';
    static readonly userCoursesPath = '/profile/courses';
    static readonly userCoursesFullPath = '/profile/courses/:type/page';
    static readonly editProfileMainPath = '/profile/edit/main';
    static readonly editProfilePasswordPath = '/profile/edit/password';
    static readonly deleteProfilePath = '/profile/edit/delete';
    static readonly courseViewPath = '/course';
    static readonly courseViewFullPath = `${this.courseViewPath}/:courseId`;
    static readonly courseParticipantsPath = `${this.courseViewPath}/:courseId/participants/page`;
    static readonly moduleViewPath = '/modules';
    static readonly moduleViewFullPath = `${this.courseViewFullPath}${this.moduleViewPath}/:moduleId`;

    static getCourseViewFullPath = (courseId: number) => `${this.courseViewPath}/${courseId}`;
    static getCourseParticipantsPath = (courseId: number) => `${this.courseViewPath}/${courseId}/participants/page`;
}