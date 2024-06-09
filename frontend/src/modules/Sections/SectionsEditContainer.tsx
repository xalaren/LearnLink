import { useParams } from "react-router-dom";
import { useAppSelector } from "../../hooks/redux";
import { Section } from "../../models/section";
import { useEffect, useState } from "react";
import { useChangeSectionOrder, useGetLessonSections, useRemoveSection } from "../../hooks/lessonSectionHook";
import { ErrorModal } from "../../components/Modal/ErrorModal";
import { Loader } from "../../components/Loader/Loader";
import SectionEditor from "./SectionEditor";
import SectionView from "../../components/Sections/SectionView";
import TextContent from "../../components/Content/TextContent";
import FileContent from "../../components/Content/FileContent";
import { FileInfo } from "../../models/fileInfo";
import CodeContent from "../../components/Content/CodeContent";
import PopupNotification from "../../components/PopupNotification";
import { NotificationType } from "../../models/enums";
import MiniLoader from "../../components/Loader/MiniLoader";

interface ISectionEditContainerProps {
    onChange: () => void;
}

function SectionsEditContainer({ onChange }: ISectionEditContainerProps) {
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
                sections.map(section =>
                    <SectionSelector
                        lessonId={Number(param.lessonId)}
                        section={section}
                        key={section.id}
                        updateCallback={onChange}
                        accessToken={accessToken}
                    />)
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
    lessonId: number,
    section: Section;
    accessToken: string;
    updateCallback: () => void;
}

function SectionSelector({ lessonId, section, updateCallback, accessToken }: ISectionSelectorProps) {
    const [editorMode, setEditorMode] = useState(false);

    const sectionRemoveHook = useRemoveSection();
    const changeOrderHook = useChangeSectionOrder();

    useEffect(() => {
        if (changeOrderHook.success) {
            resetValues();
            updateCallback();
        }
    }, [changeOrderHook.success])

    async function removeSection() {
        if (!accessToken) return;

        await sectionRemoveHook.sectionRemoveQuery(lessonId, section.id, accessToken);
    }

    async function changeOrder(increase: boolean) {
        if (!accessToken) return;

        await changeOrderHook.changeSectionOrder(section.id, lessonId, increase, accessToken);
    }

    function resetValues() {
        sectionRemoveHook.resetValues();
        changeOrderHook.resetValues();
    }


    return (
        <SectionSelectorWrapper
            error={sectionRemoveHook.error || changeOrderHook.error}
            loading={sectionRemoveHook.loading}
            onSuccess={updateCallback}
            reset={resetValues}
            success={sectionRemoveHook.success}
        >
            <div className="section-editor-container">
                {editorMode ?
                    <SectionEditor
                        lessonId={lessonId}
                        currentSection={section}
                        onClose={() => {
                            setEditorMode(false);
                            updateCallback();
                        }}
                    /> :
                    <>
                        {section.content.isText &&
                            <SectionView sectionTitle={section.title || ''} key={section.id}>
                                <TextContent text={section.content.text || ''} key={section.id} />
                            </SectionView>
                        }
                        {section.content.isFile &&
                            <SectionView sectionTitle={section.title || ''} key={section.id}>
                                <FileContent key={section.id}>
                                    {[
                                        new FileInfo(section.content.fileName!, section.content.fileExtension!, section.content.fileUrl!)
                                    ]}
                                </FileContent>
                            </SectionView>
                        }
                        {section.content.isCodeBlock &&
                            <SectionView sectionTitle={section.title || ''} key={section.id}>
                                <CodeContent language={section.content.codeLanguage || ''} key={section.id}>
                                    {section.content.text || ''}
                                </CodeContent>
                            </SectionView>
                        }
                    </>
                }

                <nav className="section-editor-container__actions">
                    <button
                        className={`control-nav__small-button button-gray-violet icon ${editorMode ? 'icon-visible' : 'icon-pen'}`}
                        onClick={() => { setEditorMode(prev => !prev) }}
                    ></button>
                    <button
                        className="control-nav__small-button button-gray-violet icon icon-cross"
                        onClick={removeSection}
                    ></button>
                    <button
                        className="control-nav__small-button button-gray-violet icon-arrow-up"
                        onClick={() => { changeOrder(true) }}
                    ></button>
                    <button
                        className="control-nav__small-button button-gray-violet icon icon-arrow-down"
                        onClick={() => { changeOrder(false) }}
                    ></button>
                </nav>
            </div>
        </SectionSelectorWrapper>
    )
}

interface ISectionSelectorWrapperProps {
    error: string;
    success: string;
    loading: boolean;
    children: React.ReactNode;
    reset: () => void;
    onSuccess: () => void;
}

function SectionSelectorWrapper({ error, success, loading, reset, onSuccess, children }: ISectionSelectorWrapperProps) {
    if (error) {
        return (
            <>
                {children}
                <PopupNotification notificationType={NotificationType.error} onFade={reset}>
                    {error}
                </PopupNotification>
            </>

        );
    }

    if (success) {
        return (
            <>
                <MiniLoader />
                <PopupNotification notificationType={NotificationType.success} onFade={() => {
                    reset();
                    onSuccess();
                }}>
                    {success}
                </PopupNotification>
            </>
        );
    }

    if (loading) {
        return (<MiniLoader />);
    }

    return (children);
}

export default SectionsEditContainer;