import { MainContainer } from "../components/MainContainer";

export function InvalidPage() {
    const styles = {
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        height: "100%"
    }
    return (
        <MainContainer styles={styles}>
            <h2 style={styles}>Страница не найдена :(</h2>
        </MainContainer>
    );
}