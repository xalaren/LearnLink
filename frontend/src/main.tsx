import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import { BrowserRouter } from "react-router-dom";
import { Provider } from 'react-redux';
import { persistor, setupStore } from './store/store.ts';
import { PersistGate } from 'redux-persist/integration/react';

const store = setupStore();

ReactDOM.createRoot(document.getElementById('root')!).render(
    <Provider store={store}>
        <BrowserRouter>
            <App />
        </BrowserRouter>
    </Provider>
)
