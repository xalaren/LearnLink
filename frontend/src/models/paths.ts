import { ProfileEditActions, ViewTypes } from "./enums";
// export class Paths {
//     static readonly homePath = '/';
//     static readonly publicPath = '/public/page';
//     static readonly loginPath = '/login';
//     static readonly registerPath = '/register';
//     static readonly profilePath = '/profile';
//     static readonly userCoursesPath = '/profile/courses';
//     static readonly userCoursesFullPath = '/profile/courses/:type/page';
//     static readonly editProfileMainPath = '/profile/edit/main';
//     static readonly editProfilePasswordPath = '/profile/edit/password';
//     static readonly deleteProfilePath = '/profile/edit/delete';
//     static readonly courseViewPath = '/course';
//     static readonly courseViewFullPath = `${this.courseViewPath}/:courseId`;
//     static readonly courseParticipantsPath = `${this.courseViewPath}/:courseId/participants/page`;
//     static readonly moduleViewPath = '/modules';
//     static readonly moduleViewFullPath = `${this.courseViewFullPath}${this.moduleViewPath}/:moduleId`;
//     static readonly courseRolesPath = `${this.courseViewFullPath}/roles`;


//     static getUserCoursesFullPath = () => `${this.userCoursesPath}/${ViewTypes.created}/page/1`
//     static getCourseViewFullPath = (courseId: number) => `${this.courseViewPath}/${courseId}`;
//     static getCourseParticipantsPath = (courseId: number) => `${this.courseViewPath}/${courseId}/participants/page`;
//     static getCourseRolesPath = (courseId: number) => `${this.courseViewPath}/${courseId}/roles`;
// }

export const paths = {
    home: '/',
    public: (pageNumber: string | number = '1') => `/public/page/${pageNumber}`,
    login: '/login',
    register: '/register',
    profile: {
        base: '/profile',
        edit: (action: ProfileEditActions | string = ProfileEditActions.main) => `/profile/edit/${action}`,
        courses: (type: ViewTypes | string = ViewTypes.created, page: string | number = '1') => `/profile/${type}/page/${page}`
    },
    course: {
        base: '/course',
        view: (courseId: string | number = '0') => `/course/${courseId}`,
        participants: (courseId: string | number, page: string | number = '1') => `/course/${courseId}/participants/page/${page}`,
        roles: (courseId: string | number) => `/course/${courseId}/roles`,
        module: {
            base: '/module',
            view: (courseId: number, moduleId: number | string) => `/course/${courseId}/module/${moduleId}`
        }
    }
}