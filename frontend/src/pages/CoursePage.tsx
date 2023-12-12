import { useParams } from "react-router-dom";
import { MainContainer } from "../components/MainContainer";
import CourseView from "../modules/CourseView";
import { useEffect, useState } from "react";

function CoursePage() {
    const param = useParams<'id'>();
    const [courseId, setCourseId] = useState<number>(0);

    useEffect(() => {
        if (param.id) {
            setCourseId(Number(param.id));
        }
        else {
            setCourseId(0);
        }
    }, [param.id])

    return (
        <MainContainer>
            <CourseView courseId={courseId} />
        </MainContainer>
    );
}

export default CoursePage;