import { useParams } from "react-router-dom";
import { useAppSelector } from "../../hooks/redux";
import { Section } from "../../models/section";
import { useEffect, useState } from "react";
import { useGetLessonSections } from "../../hooks/sectionHooks";
import { ErrorModal } from "../../components/Modal/ErrorModal";
import { Loader } from "../../components/Loader/Loader";
import SectionView from "../../components/Sections/SectionView";
import TextContent from "../../components/Content/TextContent";
import FileContent from "../../components/Content/FileContent";
import { FileInfo } from "../../models/fileInfo";
import CodeContent from "../../components/Content/CodeContent";

function SectionsViewContainer() {
    const param = useParams<'lessonId'>();
    const { accessToken } = useAppSelector(state => state.authReducer);

    const [sections, setSections] = useState<Section[]>();
    const { getSectionsFromLesson, error, loading, resetValues } = useGetLessonSections();

    useEffect(() => {
        fetchSections();
    }, [param, accessToken])

    async function fetchSections() {
        if (!param || !accessToken) return;

        const result = await getSectionsFromLesson(Number(param.lessonId), accessToken);

        if (result) setSections(result);
    }

    return (
        <>
            {!error && !loading && sections &&
                sections.map(section => <SectionSelector section={section} key={section.id} />)
            }

            {!error && !loading && (!sections || sections.length == 0) &&
                <p>Содержимое урока отсутствует...</p>
            }

            {loading && !error &&
                <Loader />
            }
            {error &&
                <ErrorModal active={Boolean(error)} error={error} onClose={resetValues} />
            }
        </>
    );
}

interface ISectionSelectorProps {
    section: Section;
}

function SectionSelector({ section }: ISectionSelectorProps) {
    if (section.content.isText) {
        return (
            <SectionView sectionTitle={section.title || `Раздел ${section.order}`}>
                <TextContent text={section.content.text || ''} />
            </SectionView>
        );
    }

    if (section.content.isFile) {
        return (
            <SectionView sectionTitle={section.title || `Раздел ${section.order}`}>
                <FileContent>
                    {[
                        new FileInfo(section.content.fileName!, section.content.fileExtension!, section.content.fileUrl!)
                    ]}
                </FileContent>
            </SectionView>
        );
    }

    if (section.content.isCodeBlock) {
        return (
            <SectionView sectionTitle={section.title || `Раздел ${section.order}`}>
                <CodeContent language={section.content.codeLanguage || ''}>
                    {section.content.text || ''}
                </CodeContent>
            </SectionView>
        );
    }
}

export default SectionsViewContainer;