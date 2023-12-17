export class Subscription {
    userId: number;
    courseId: number;
    startDate: Date;

    constructor(userId: number, courseId: number, startDate = new Date()) {
        this.userId = userId;
        this.courseId = courseId
        this.startDate = startDate;
    }
}