import CreditCardList from '../creditcards/CreditCardList'
import './App.css'
import Header from './Header'
import {
  BrowserRouter as Router,
  Route,
  Routes
} from "react-router-dom"
import { slide as Menu } from 'react-burger-menu'
import RegisterCreditCard from '../creditcards/RegisterCreditCard'

function App() {
  return (
    <Router>      
      <Menu noOverlay noTransition disableAutoFocus disableOverlayClick width={ '20%' }>
        <a id="home" className="bm-item" href="/">Home</a>
        <a id="registeredCards" className="bm-item" href="/cards">List Cards</a>
        <a id="addCard" className="bm-item" href="/register">Register Card</a>
      </Menu>
      <Header title='Credit Cards Manager'/>
      <Routes>
        <Route path="/cards" element={<CreditCardList />}></Route>
        <Route path="/" element={<Header />}></Route>
        <Route path="/register" element={<RegisterCreditCard />}></Route>
      </Routes>
  </Router>
  )
}

export default App
