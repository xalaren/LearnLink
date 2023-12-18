import { useParams } from "react-router-dom";
import { MainContainer } from "../components/MainContainer";
import CourseView from "../modules/CourseView";
import { useEffect, useState } from "react";

function CoursePage() {
    const param = useParams<'courseId'>();
    const [courseId, setCourseId] = useState<number>(0);

    useEffect(() => {
        if (param.courseId) {
            setCourseId(Number(param.courseId));
        }
        else {
            setCourseId(0);
        }
    }, [param.courseId])

    return (
        <MainContainer>
            <CourseView courseId={courseId} />
        </MainContainer>
    );
}

export default CoursePage;