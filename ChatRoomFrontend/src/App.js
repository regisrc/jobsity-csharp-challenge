import { BrowserRouter } from "react-router-dom";

import { ThemeProvider as StyledProvider } from "styled-components";

import GlobalStyle from "./styles/global";
import RoutesApp from "./routes/index";
import { theme } from "./styles/theme";

const App = () => {
  return (
    <BrowserRouter>
      <StyledProvider theme={theme}>
        <GlobalStyle />

        <RoutesApp />
      </StyledProvider>
    </BrowserRouter>
  );
};

export default App;