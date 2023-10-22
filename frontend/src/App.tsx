import {Route, Routes} from 'react-router-dom'
import {Header} from "./components/Header.tsx";
import {PublicPage} from "./components/PublicPage.tsx";
import "./css/index.css"
function App() {
  return (
    <>
        <Header/>

        <Routes>
            <Route path="/" element={<PublicPage/>}/>
        </Routes>
    </>
  )
}

export default App
