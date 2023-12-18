import { useParams } from "react-router-dom";
import ModuleView from "../modules/ModuleView";
import { useEffect, useState } from "react";
import { MainContainer } from "../components/MainContainer";

function ModulePage() {
    const courseParam = useParams<'courseId'>();
    const moduleParam = useParams<'moduleId'>();

    const [courseId, setCourseId] = useState<number>(0);
    const [moduleId, setModuleId] = useState<number>(0);

    useEffect(() => {
        if (courseParam.courseId && moduleParam.moduleId) {
            setCourseId(Number(courseParam.courseId));
            setModuleId(Number(moduleParam.moduleId));
        }
        else {
            setCourseId(0);
            setModuleId(0);
        }
    }, [courseParam.courseId, moduleParam.moduleId]);


    return (
        <MainContainer>
            <ModuleView courseId={courseId} moduleId={moduleId} />
        </MainContainer>
    );
}

export default ModulePage;