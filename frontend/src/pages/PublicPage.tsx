import { MainContainer } from "../components/MainContainer";
import { Loader } from "../ui/Loader";

export function PublicPage() {
    return (
        <MainContainer title="Общедоступные курсы">
            <Loader className="loader-center" />
        </MainContainer>
    )
}